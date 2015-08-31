Namespace OptimOrm.Translators.DataSource

    Public MustInherit Class ANSI92Translator
        Inherits SQLTranslator

        Public MustOverride Overrides Function parseDate(ByVal value As Date) As String


        Public Overrides Function parseNumber(ByVal value As Object) As String
            Return value.ToString().Replace(Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, "").Replace(Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".")
        End Function


        Public Overrides Function getInsertQuery(ByVal obj As Model) As String

            Dim td As TableDefinition = ModelTranslator.getTableDefinition(obj)
            Dim res As New System.Text.StringBuilder()

            res.Append("insert into " & td.tableName & "(")

            For Each e As FieldDefinition In td.fields
                If Not e.Calculated Then res.Append(e.Name & ",") 'se agregan lo que no sean se sólo lectura
            Next
            res.Remove(res.Length - 1, 1) 'elimina la última coma
            res.Append(") values (")

            For Each e As FieldDefinition In td.fields
                If Not e.Calculated Then res.Append(Me.getObjectValue(obj, e.Name) & ",") 'PARSEAR
            Next
            res.Remove(res.Length - 1, 1)
            res.Append(");")

            Return res.ToString()
        End Function

        Public Overrides Function getUpdateQuery(ByVal obj As Model) As String

            Dim td As TableDefinition = ModelTranslator.getTableDefinition(obj)
            Dim res As New System.Text.StringBuilder()

            res.Append("update ") : res.Append(td.tableName) : res.Append(" set ")

            For Each e As FieldDefinition In td.fields
                If e.Name <> td.primaryKey Then
                    If Not e.Calculated Then res.Append(e.Name & "=" & Me.getObjectValue(obj, e.Name).ToString() & ",")
                End If
            Next

            res.Remove(res.Length - 1, 1) 'elimina la última coma
            res.Append(" where ") : res.Append(td.primaryKey) : res.Append("=") : res.Append(Me.getObjectValue(obj, td.primaryKey))
            res.Append(";")

            Return res.ToString()
        End Function

        Public Overrides Function getLoadQuery(ByVal obj As Model, ByVal pkValue As Object) As String

            Dim td As TableDefinition = ModelTranslator.getTableDefinition(obj)
            Dim res As New System.Text.StringBuilder()

            res.Append("select ")

            For Each e As FieldDefinition In td.fields
                res.Append(e.Name & ",")
            Next
            res.Remove(res.Length - 1, 1) 'elimina la última coma
            res.Append(" from  ")
            res.Append(td.tableName)
            res.Append(" where ")
            res.Append(td.primaryKey)
            res.Append("=")
            res.Append("'" & pkValue.ToString() & "'") 'PARSEAR
            res.Append(";")

            Return res.ToString()
        End Function

        Public Overrides Function getFindQuery(ByVal obj As Model, ByVal criteria As String, ByVal values() As Object) As String

            Dim td As TableDefinition = ModelTranslator.getTableDefinition(obj)
            Dim res As New System.Text.StringBuilder()

            res.Append("select ")

            For Each e As FieldDefinition In td.fields
                res.Append(e.Name & ",")
            Next
            res.Remove(res.Length - 1, 1) 'elimina la última coma
            res.Append(" from  ")
            res.Append(td.tableName)

            res.Append(" ") 'un espacio vacío para que el usuario no tenga que escribirlo en la OQL

            Dim repl As String = ""
            For i As Integer = 0 To values.Length - 1
                If values(i) Is Nothing Then repl = "null" Else repl = values(i).ToString()
                criteria = criteria.Replace("?" & (i + 1).ToString(), repl) 'reemplaza el i-ésimo+1 parámetro por el iésimo valor
            Next
            res.Append(criteria)

            res.Append(";")

            Return res.ToString()
        End Function

        Public Overrides Function getDeleteQuery(ByVal obj As Model, ByVal pkValue As Object) As String
            Return "DELETE from " & obj.TableName & " where " & obj.PrimaryKey() & "=" & Me.getAttributeValue(pkValue) & ";"
        End Function

    End Class

End Namespace
