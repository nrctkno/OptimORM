Namespace OptimOrm.Translators.Code


    Public Class VBTranslator
        Inherits DotNetTranslator

        Public Overrides Sub makeProperty(ByVal name As String, ByVal var As String, ByVal type As String, ByVal sb As System.Text.StringBuilder)
            With sb
                .AppendLine(" Public Property " & name & "() as " & type)
                .AppendLine("  Get")
                .AppendLine("   return " & var)
                .AppendLine("  End Get")
                .AppendLine("  Set(value as " & type & ")")
                .AppendLine("   Me." & var & "=" & "value")
                .AppendLine("  End Set")
                .AppendLine(" End Property")
            End With

        End Sub

        Public Overrides Function getFileExtension() As String
            Return "vb"
        End Function

    End Class


End Namespace