Namespace OptimOrm.Adapters

    ''' <summary>
    ''' Adapta un datareader a un objeto de modelo
    ''' </summary>
    Public Class ModelAdapter
        Inherits Adapter

        Public model As Model

        Public Sub New(ByVal m As Model)
            Me.model = m
        End Sub

        Public Overrides Sub Fill(ByVal reader As System.Data.IDataReader)
            If reader.Read() Then
                Dim td As TableDefinition = ModelTranslator.getTableDefinition(Me.model)

                For Each fd As FieldDefinition In td.fields
                    If reader(fd.Name).GetType() Is GetType(System.DBNull) Then
                        Me.model.GetType.GetProperty(fd.Name, fd.Type).SetValue(Me.model, Nothing, Nothing)
                    Else
                        Me.model.GetType.GetProperty(fd.Name, fd.Type).SetValue(Me.model, reader(fd.Name), Nothing)
                    End If
                Next
            End If
        End Sub

    End Class

End Namespace
