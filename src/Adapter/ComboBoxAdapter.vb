Namespace OptimOrm.Adapters

    ''' <summary>
    ''' adapta una DataReader a un combo
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ComboBoxAdapter(Of T As {OptimOrm.IDescriptor, New})
        Inherits Adapter

        Private combo As ComboBox

        Public Sub New(ByVal combo As ComboBox)
            Me.combo = combo
        End Sub

        Public Overrides Sub Fill(ByVal reader As System.Data.IDataReader)
            Me.combo.BeginUpdate()
            Me.combo.Items.Clear()

            Dim d As OptimOrm.IDescriptor, w As Integer ' se usa para redimensionar el ancho del desplegable cel combo


            While reader.Read 'agregamos las filas leídas
                d = New T() 'Activator.CreateInstance(GetType(DTO.Dato)) 'New OptimOrm.Descriptor()
                d.fillFromDataReader(reader)
                Me.combo.Items.Add(d)
                w = TextRenderer.MeasureText(Me.combo.Items(Me.combo.Items.Count - 1).ToString(), combo.Font).Width
                If w > combo.DropDownWidth Then combo.DropDownWidth = w + 20
            End While

            Me.combo.EndUpdate()

        End Sub

        Public Shared Sub selectById(ByVal combo As ComboBox, ByVal key As Object)
            For Each d As OptimOrm.IDescriptor In combo.Items
                If d.getKey().Equals(key) Then combo.SelectedItem = d
            Next
        End Sub


    End Class


End Namespace
