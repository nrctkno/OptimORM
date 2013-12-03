Namespace OptimOrm.Adapters

    ''' <summary>
    ''' Adapta un datareader a una cadena
    ''' </summary>
    Public Class StringAdapter
        Inherits Adapter

        Public fieldSeparator As String = vbTab
        Public result As System.Text.StringBuilder

        Public Sub New()
        End Sub

        Public Overrides Sub Fill(ByVal reader As System.Data.IDataReader)

            Me.result = New System.Text.StringBuilder()

            While reader.Read 'agregamos las filas leídas
                For I As Integer = 0 To (reader.FieldCount - 1)
                    Me.result.Append(reader(I))
                    Me.result.Append(Me.fieldSeparator)
                Next
                Me.result.AppendLine()
            End While

        End Sub

    End Class

End Namespace
