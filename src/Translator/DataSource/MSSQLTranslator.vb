Namespace OptimOrm.Translators.DataSource

    Public Class MSSQLTranslator
        Inherits ANSI92Translator


        Public Overrides Function parseDate(ByVal value As Date) As String
            Dim F As String = ""
            If value <> New Date() Then
                With value
                    If .Year < 10 Then F &= "20"
                    F &= .Year
                    If .Month < 10 Then F &= "0"
                    F &= .Month
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
            Return "SELECT c.name 'Field', t.Name 'Type', c.is_nullable 'Null', i.is_primary_key as 'Key', i.type_desc as 'Extra' FROM sys.columns c INNER JOIN sys.types t ON c.system_type_id = t.system_type_id LEFT JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id LEFT JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id WHERE c.object_id = OBJECT_ID('" & tableName & "') and t.name!='sysname';"
        End Function

        Public Overrides Function getQueryForListAllTables() As String
            Return "Select Table_name From Information_schema.Tables Where Table_type = 'BASE TABLE' and Objectproperty (Object_id(Table_name), 'IsMsShipped') = 0 order by Table_name;"
        End Function

        Public Overrides Function isKey(ByVal columnName As String) As String
            Return (columnName = True)
        End Function

        Public Overrides Function getQueryForListAllChildrenRelationships(ByVal tableName As String) As String
            Return "SELECT pt=PK.TABLE_NAME,pc=PT.COLUMN_NAME,ft=FK.TABLE_NAME,fc=CU.COLUMN_NAME,c=C.CONSTRAINT_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME INNER JOIN (SELECT i1.TABLE_NAME, i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY') PT ON PT.TABLE_NAME = PK.TABLE_NAME where PK.TABLE_NAME='" & tableName & "';"
        End Function

        Public Overrides Function getQueryForListAllParentRelationships(ByVal tableName As String) As String
            Return "SELECT pt=PK.TABLE_NAME,pc=PT.COLUMN_NAME,ft=FK.TABLE_NAME,fc=CU.COLUMN_NAME,c=C.CONSTRAINT_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME INNER JOIN (SELECT i1.TABLE_NAME, i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY') PT ON PT.TABLE_NAME = PK.TABLE_NAME where FK.TABLE_NAME='" & tableName & "';"
        End Function


    End Class


End Namespace
