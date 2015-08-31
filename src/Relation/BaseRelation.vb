Namespace OptimOrm

    Public MustInherit Class BaseRelation
        Implements IRelation


        Public MustOverride Function SaveAll() As Boolean Implements IRelation.SaveAll
        Public MustOverride Function DeleteAll() As Boolean Implements IRelation.DeleteAll


        Public local As Model
        Public referenced As Type
        Public lk As String
        Public rk As String
        Public collection As List(Of Model)


        ''' <summary>Crea una nueva instancia de Relation</summary>
        ''' <param name="local">el modelo para el que se está creando la relación</param>
        ''' <param name="lk">el nombre del campo local que es clave foránea de remote</param>
        ''' <param name="referenced">el tipo que define al/a los modelo/s referenciados</param>
        ''' <param name="rk">el nombre del campo del ó los modelos referenciados que se corresponde con el campo local</param>
        Public Sub New(ByVal local As Model, ByVal lk As String, ByVal referenced As Type, ByVal rk As String)
            Me.local = local
            Me.lk = lk
            Me.referenced = referenced
            Me.rk = rk
            Me.collection = New List(Of Model)
        End Sub

        Public Function Fill() As Boolean Implements IRelation.Fill
            Dim obj = Activator.CreateInstance(Me.referenced)
            Dim params As New List(Of Object)

            params.Add(Me.local.GetType.GetProperty(Me.lk).GetValue(Me.local, Nothing))
            Me.collection = Connection.getInstance().Find("where " & Me.rk & "='?1'", params.ToArray(), obj.GetType)
        End Function

        Public Function GetAll() As List(Of Model) Implements IRelation.GetAll
            Return Me.collection
        End Function

        Public Function GetUnique() As Model Implements IRelation.GetUnique
            If Me.collection.Count > 0 Then Return Me.collection(0) Else Return Nothing
        End Function

        Public Sub AddUnique(ByVal m As Model) Implements IRelation.AddUnique
            Me.collection.Clear()
            Me.local.GetType().GetProperty(Me.lk).SetValue(Me.local, m.GetType().GetProperty(Me.rk).GetValue(m, Nothing), Nothing)
            Me.collection.Add(m)
        End Sub

        Public Sub AddAsForeign(ByVal m As Model) Implements IRelation.AddAsForeign
            m.GetType().GetProperty(Me.rk).SetValue(m, Me.local.GetType().GetProperty(Me.lk).GetValue(Me.local, Nothing), Nothing)
            Me.collection.Add(m)
        End Sub


        Public Sub dump(ByVal nestingLevel As Integer, ByVal w As System.IO.TextWriter) Implements IRelation.dump
            w.WriteLine(Space(nestingLevel) & " " & Me.GetType().Name & " " & Me.local.GetType().Name & "-" & Me.referenced.Name & "(" & Me.lk & "-" & Me.rk & ")")
            nestingLevel += 2
            For Each m As Model In Me.collection
                m.dump(nestingLevel, w)
            Next
        End Sub


    End Class


End Namespace