Namespace OptimOrm.Translators.DataSource

    Public Class MySQLTranslator
        Inherits ANSI92Translator


        Public Overrides Function parseDate(ByVal value As Date) As String
            Dim F As String = ""
            If value <> New Date() Then
                With value
                    If .Year < 10 Then F &= "20"
                    F &= .Year
                    F &= "-"
                    If .Month < 10 Then F &= "0"
                    F &= .Month
                    F &= "-"
                    If .Day < 10 Then F &= "0"
                    F &= .Day
                    F &= " "
                    If .Hour < 10 Then F &= "0"
                    F &= .Hour
                    F &= ":"
                    If .Minute < 10 Then F &= "0"
                    F &= .Minute
                    F &= ":"
                    If .Second < 10 Then F &= "0"
                    F &= .Second
                End With
                Return "'" & F & "'"
            Else
                Return "null"
            End If
        End Function

        Public Overrides Function getQueryForListAllColumns(ByVal tableName As String) As String
            Return "show columns from " & tableName & ";"
        End Function

        Public Overrides Function getQueryForListAllTables() As String
            Return "show tables;"
        End Function

        Public Overrides Function isKey(ByVal columnName As String) As String
            Return (columnName = "PK")
        End Function

        Public Overrides Function getQueryForListAllChildrenRelationships(ByVal tableName As String) As String
            Return "SELECT referenced_table_name as pt, referenced_column_name as pc, table_name as ft, column_name as fc FROM INFORMATION_SCHEMA.key_column_usage WHERE referenced_table_name IS NOT NULL AND referenced_table_name='" & tableName & "';"  '"AND referenced_table_schema = '" & tableName & "'"
        End Function

        Public Overrides Function getQueryForListAllParentRelationships(ByVal tableName As String) As String
            Return "SELECT referenced_table_name as pt, referenced_column_name as pc, table_name as ft, column_name as fc FROM INFORMATION_SCHEMA.key_column_usage WHERE referenced_table_name IS NOT NULL AND table_name= '" & tableName & "'"
        End Function


    End Class


End Namespace
