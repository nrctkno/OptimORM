Namespace OptimOrm


    Public Class Relation
        Inherits BaseRelation

        ''' <summary>Crea una nueva instancia de Relation</summary>
        ''' <param name="local">el modelo para el que se está creando la relación</param>
        ''' <param name="lk">el nombre del campo local que es clave foránea de remote</param>
        ''' <param name="referenced">el tipo que define al/a los modelo/s referenciados</param>
        ''' <param name="rk">el nombre del campo del ó los modelos referenciados que se corresponde con el campo local</param>
        Public Sub New(ByVal local As Model, ByVal lk As String, ByVal referenced As Type, ByVal rk As String)
            MyBase.New(local, lk, referenced, rk)
        End Sub

        Public Overrides Function SaveAll() As Boolean
            For Each obj As Model In Me.collection
                If Not obj.Save() Then Return False
            Next
            Return True
        End Function

        Public Overrides Function DeleteAll() As Boolean
            For Each obj As Model In Me.collection
                If Not obj.Delete() Then Return False
            Next
            Return True
        End Function

    End Class


End Namespace