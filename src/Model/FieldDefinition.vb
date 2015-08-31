Namespace OptimOrm


    Public Class FieldDefinition

        Public Name As String
        Public Type As Type
        Public Calculated As Boolean

        Public Sub New(ByVal name As String, ByVal type As System.Type, ByVal calculated As Boolean)
            Me.Name = name
            Me.Type = type
            Me.Calculated = calculated
        End Sub

    End Class


End Namespace