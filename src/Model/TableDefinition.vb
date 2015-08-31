Namespace OptimOrm


    Public Class TableDefinition

        Public tableName As String
        Public primaryKey As String
        Public fields As List(Of FieldDefinition)

        Public Sub New()
            Me.fields = New List(Of FieldDefinition)
        End Sub

    End Class


End Namespace