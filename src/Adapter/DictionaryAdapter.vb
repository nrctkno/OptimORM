Namespace OptimOrm.Adapters

    ''' <summary>
    ''' Adapta un datareader a una lista de campos
    ''' </summary>
    Public Class KeyValueAdapter
        Inherits Adapter

        Dim container As List(Of Dictionary(Of String, Object))

        ''' <summary>
        ''' Crea una nueva instancia de DictionaryAdapter
        ''' </summary>
        ''' <param name="container">un diccionario</param>
        Public Sub New(ByVal container As List(Of Dictionary(Of String, Object)))
            Me.container = container
        End Sub

        Public Overrides Sub Fill(ByVal reader As System.Data.IDataReader)

            While reader.Read 'agregamos las filas leídas
                'creamos el item a agregar
                Dim e As New Dictionary(Of String, Object)
                'agregamos los subcampos al item
                For I As Integer = 0 To (reader.FieldCount - 1)
                    e.Add(reader.GetName(I), reader(I))
                Next
                Me.container.Add(e)
            End While

        End Sub


        Public Class BaseDecorator

            Public Overridable Sub DecorateItem(ByRef i As ListViewItem)
            End Sub

        End Class

    End Class

End Namespace
