Namespace OptimOrm.Translators.DataSource

    Public MustInherit Class SQLTranslator


        Public MustOverride Function parseNumber(ByVal obj As Object) As String
        Public MustOverride Function parseDate(ByVal value As Date) As String

        Public MustOverride Function getQueryForListAllTables() As String
        Public MustOverride Function getQueryForListAllColumns(ByVal tableName As String) As String
        Public MustOverride Function getQueryForListAllChildrenRelationships(ByVal tableName As String) As String
        Public MustOverride Function getQueryForListAllParentRelationships(ByVal tableName As String) As String
        Public MustOverride Function isKey(ByVal columnName As String) As String

        Public MustOverride Function getInsertQuery(ByVal obj As Model) As String
        Public MustOverride Function getUpdateQuery(ByVal obj As Model) As String
        Public MustOverride Function getDeleteQuery(ByVal obj As Model, ByVal pkValue As Object) As String
        Public MustOverride Function getLoadQuery(ByVal obj As Model, ByVal pkValue As Object) As String
        Public MustOverride Function getFindQuery(ByVal obj As Model, ByVal criteria As String, ByVal values() As Object) As String


        Public Overridable Function translateType(ByVal type As String) As String
            If (type.IndexOf("char") >= 0) Or (type.IndexOf("text") >= 0) Or (type.IndexOf("binary") >= 0) Or (type.IndexOf("blob") >= 0) Then
                Return "String"
            ElseIf type.IndexOf("int") >= 0 Then
                Return "Integer"
            ElseIf (type.IndexOf("decimal") >= 0) Or (type.IndexOf("float") >= 0) Or (type.IndexOf("real") >= 0) Then
                Return "Single"
            ElseIf (type.IndexOf("double") >= 0) Then
                Return "Double"
            ElseIf (type.IndexOf("date") >= 0) Or (type.IndexOf("time") >= 0) Then
                Return "DateTime"
            Else
                Return "Object"
            End If

        End Function

        Public Function getAttributeValue(ByVal value As Object) As String
            If value Is Nothing Then
                Return ("null")
            Else
                If TypeOf (value) Is DateTime Then
                    Return Me.parseDate(value)
                ElseIf IsNumeric(value) Then
                    Return Me.parseNumber(value)
                Else
                    Return "'" & value.ToString() & "'"
                End If
            End If
        End Function

        Public Function getObjectValue(ByVal obj As Object, ByVal propertyName As String) As String
            Dim p As Reflection.PropertyInfo = obj.GetType.GetProperty(propertyName)
            Dim value As Object = p.GetValue(obj, Nothing)

            If value Is Nothing Then
                Return ("null")
            Else
                If TypeOf (value) Is DateTime Then
                    Return Me.parseDate(value)
                ElseIf IsNumeric(value) Then
                    Return Me.parseNumber(value)
                Else
                    Return "'" & value.ToString() & "'"
                End If
            End If
        End Function


        Public Function injectParams(ByVal sql As String, ByVal params As Object()) As String
            For i As Integer = 0 To params.Length - 1
                sql = sql.Replace("?" & (i + 1).ToString(), params(i).ToString()) 'reemplaza el i-ésimo+1 parámetro por el iésimo valor
            Next
            Console.Out.WriteLine(sql)
            Return sql
        End Function

    End Class

End Namespace
