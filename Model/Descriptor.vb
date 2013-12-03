Namespace OptimOrm


    Public Interface IDescriptor

        Sub fillFromDataReader(ByVal r As IDataReader)
        Function getKey() As Object
        Function ToString() As String

    End Interface


End Namespace