Namespace OptimOrm


    Public Class ModelTranslator


        Public Shared Function getTableDefinition(ByVal mdl As Model) As TableDefinition
            Dim def As New TableDefinition
            def.tableName = mdl.TableName
            def.primaryKey = mdl.PrimaryKey

            For Each pi As Reflection.PropertyInfo In mdl.GetType.GetProperties()
                def.fields.Add(New FieldDefinition(pi.Name, pi.PropertyType, AttributeHasCategory(pi, "calculated")))
            Next

            Return def
        End Function

        Private Shared Function AttributeHasCategory(ByVal pi As Reflection.PropertyInfo, ByVal category As String)
            Dim attrs As Object() = pi.GetCustomAttributes(GetType(System.ComponentModel.CategoryAttribute), True)
            For Each a As Object In attrs
                If CType(a, System.ComponentModel.CategoryAttribute).Category = category Then Return True
            Next
            Return False
        End Function


    End Class


End Namespace