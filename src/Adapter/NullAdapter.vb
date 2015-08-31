Namespace OptimOrm.Adapters

    ''' <summary>
    ''' Un adaptador que no hace nada
    ''' </summary>
    Public Class NullAdapter
        Inherits Adapter

        Public Overrides Sub Fill(ByVal reader As System.Data.IDataReader)

        End Sub

    End Class

End Namespace
