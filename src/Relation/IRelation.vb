Namespace OptimOrm


    ''' <summary>Representa una relación</summary>
    Public Interface IRelation

        Function Fill() As Boolean
        Function SaveAll() As Boolean
        Function DeleteAll() As Boolean
        Function GetAll() As List(Of Model)
        Function GetUnique() As Model
        ''' <summary>Agrega un modelo relacionado, que será único (relación varios-a-uno)</summary>
        Sub AddUnique(ByVal m As Model)
        ''' <summary>Agrega un modelo relacionado (relación uno-a-varios)</summary>
        Sub AddAsForeign(ByVal m As Model)
        Sub dump(ByVal nestingLevel As Integer, ByVal w As System.IO.TextWriter)

    End Interface


End Namespace