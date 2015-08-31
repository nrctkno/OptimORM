Namespace OptimOrm


    Public Class ReadOnlyRelation
        Inherits BaseRelation

        ''' <summary>Crea una nueva instancia de ReadOnlyRelation. Una relación de sólo lectura.</summary>
        ''' <param name="local">el modelo para el que se está creando la relación</param>
        ''' <param name="lk">el nombre del campo local que es clave foránea de remote</param>
        ''' <param name="referenced">el tipo que define al/a los modelo/s referenciados</param>
        ''' <param name="rk">el nombre del campo del ó los modelos referenciados que se corresponde con el campo local</param>
        Public Sub New(ByVal local As Model, ByVal lk As String, ByVal referenced As Type, ByVal rk As String)
            MyBase.New(local, lk, referenced, rk)
        End Sub

        ''' <summary>No hace nada, ya que no hay nada que guardar</summary>
        Public Overrides Function SaveAll() As Boolean
            Return True
        End Function

        ''' <summary>No hace nada, ya que es una relación de sólo lectura. Si no se puede crear, no se puede eliminar(justicia divina!)</summary>
        Public Overrides Function DeleteAll() As Boolean
            Return True
        End Function

    End Class


End Namespace