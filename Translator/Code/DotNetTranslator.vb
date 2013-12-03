Namespace OptimOrm.Translators.Code


    Public MustInherit Class DotNetTranslator

        Public MustOverride Sub makeProperty(ByVal name As String, ByVal var As String, ByVal type As String, ByVal w As System.Text.StringBuilder)
        Public MustOverride Function getFileExtension() As String


    End Class


End Namespace