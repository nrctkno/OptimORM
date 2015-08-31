Namespace OptimOrm.Adapters

    ''' <summary>
    ''' Un adaptador que devuelve información acerca de la consulta
    ''' </summary>
    Public Class InfoAdapter
        Inherits Adapter

        Public info As Information

        Public Sub New()
            Me.info = New Information()
        End Sub

        Public Sub New(ByVal info As Information)
            Me.info = info
        End Sub

        Public Overrides Sub Fill(ByVal reader As System.Data.IDataReader)
            While reader.Read
                Me.info.rowcount += 1
            End While
            Me.info.fieldCount = reader.FieldCount
        End Sub


        Public Class Information

            Public rowCount As Integer
            Public fieldCount As Integer

        End Class

    End Class



End Namespace
