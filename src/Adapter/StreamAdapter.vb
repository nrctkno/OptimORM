Namespace OptimOrm.Adapters

    ''' <summary>
    ''' Adapta un datareader a un flujo de datos
    ''' </summary>
    Public Class StreamAdapter
        Inherits Adapter

        Private stream As IO.StreamWriter
        ''' <summary>
        ''' Crea una nueva instancia de DictionaryAdapter
        ''' </summary>
        ''' <param name="stream">un flujo de datos</param>
        Public Sub New(ByVal stream As IO.StreamWriter)
            Me.stream = stream
        End Sub

        Public Overrides Sub Fill(ByVal reader As System.Data.IDataReader)

            While reader.Read 'agregamos las filas leídas
                'creamos el item a agregar
                'agregamos los subcampos al item
                For I As Integer = 0 To (reader.FieldCount - 1)
                    Me.stream.Write(reader.GetName(I))
                    Me.stream.Write(": ")
                    Me.stream.Write(reader(I))
                Next
                Me.stream.WriteLine()
            End While

        End Sub

    End Class

End Namespace
