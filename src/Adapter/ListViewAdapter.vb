Namespace OptimOrm.Adapters

    ''' <summary>
    ''' Adapta un datareader a un listview (llena el listview)
    ''' </summary>
    Public Class ListViewAdapter
        Inherits Adapter

        Dim listview As ListView
        Dim decorator As BaseDecorator

        Public Sub New(ByVal l As ListView)
            Me.New(l, New BaseDecorator)
        End Sub

        ''' <summary>
        ''' Crea una nueva instancia de un decorador de ítems
        ''' </summary>
        ''' <param name="l">el visor</param>
        ''' <param name="decorator">un decorador para los ítems que se agregarán</param>
        Public Sub New(ByVal l As ListView, ByVal decorator As BaseDecorator)
            Me.listview = l
            Me.decorator = decorator
        End Sub

        Public Overrides Sub Fill(ByVal reader As System.Data.IDataReader)

            Me.listview.BeginUpdate() 'detenemos el dibujado del listview
            Me.listview.Items.Clear() 'borramos sus items

            While reader.Read 'agregamos las filas leídas
                'creamos el item a agregar
                Dim It As New ListViewItem()
                'guarda la Id en el tag del item
                It.Tag = reader(0).ToString
                'muestra el primer campo como nombre del item
                It.Text = reader(1).ToString
                'agregamos los subcampos al item
                For I As Integer = 2 To (reader.FieldCount - 1)
                    It.SubItems.Add _
                        (New ListViewItem.ListViewSubItem(It, reader(I).ToString))
                Next
                Me.decorator.DecorateItem(It)
                Me.listview.Items.Add(It) 'agregamos el item a la lista
            End While
            'ajustamos las columnas
            Me.listview.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            Me.listview.EndUpdate()

        End Sub


        Public Class BaseDecorator

            Public Overridable Sub DecorateItem(ByRef i As ListViewItem)
            End Sub

        End Class

    End Class

End Namespace
