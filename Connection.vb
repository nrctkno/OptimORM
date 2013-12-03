Namespace OptimOrm


    Public Class Connection


#Region "miembros estáticos"

        Private Shared singletonConnection As Connection = Nothing


        Public Shared Sub Configure(ByVal conn As IDbConnection, ByVal translator As Translators.DataSource.SQLTranslator)
            singletonConnection = New Connection(conn, translator)
        End Sub

        Public Shared Function getInstance() As Connection
            If singletonConnection Is Nothing Then Throw New Exception("La conexión no ha sido configurada")
            Return singletonConnection
        End Function

#End Region


#Region "miembros de instancia"

        Private translator As Translators.DataSource.SQLTranslator
        Private connection As IDbConnection = Nothing
        Private currentTransaction As IDbTransaction = Nothing


        Private Sub New(ByVal conn As IDbConnection, ByVal translator As Translators.DataSource.SQLTranslator)
            Me.connection = conn
            Me.translator = translator
        End Sub

        Private Sub KillTransaction()
            Me.connection.Close()
            Me.currentTransaction.Dispose()
            Me.currentTransaction = Nothing
        End Sub

        Public Sub BeginTransaction(Optional ByVal isolation As Data.IsolationLevel = IsolationLevel.ReadCommitted)
            Me.connection.Open()
            Me.currentTransaction = Me.connection.BeginTransaction(isolation)
        End Sub

        Public Sub CommitTransaction()
            Me.currentTransaction.Commit()
            Me.KillTransaction()
        End Sub

        Public Sub RollbackTransaction()
            Me.currentTransaction.Rollback()
            Me.KillTransaction()
        End Sub

        Public Function HasTransaction() As Boolean
            Return Me.currentTransaction IsNot Nothing
        End Function


        Public Function executeNonQuery(ByVal sql As String) As Integer
            Return Me.executeNonQuery(sql, New Object() {})
        End Function

        Public Function executeNonQuery(ByVal sql As String, ByVal params As Object()) As Integer
            Dim result As Integer

            sql = Me.translator.injectParams(sql, params)

            If Not Me.HasTransaction() Then
                Me.connection.Open()
                Using c As System.Data.IDbCommand = Me.connection.CreateCommand()
                    'no se establece transacción
                    c.CommandText = sql
                    result = c.ExecuteNonQuery()
                End Using
                Me.connection.Close()
            Else
                'la conexión debería esta abierta
                Using c As System.Data.IDbCommand = Me.connection.CreateCommand()
                    c.Transaction = Me.currentTransaction
                    c.CommandText = sql
                    result = c.ExecuteNonQuery()
                End Using
                'no se debe cerrar la conexión
            End If

            Return result
        End Function


        Public Function executeQuery(ByVal sql As String, ByVal adapter As Adapters.Adapter) As Boolean
            Return Me.executeQuery(sql, New Object() {}, adapter)
        End Function

        Public Function executeQuery(ByVal sql As String, ByVal params As Object(), ByVal adapter As Adapters.Adapter) As Boolean
            sql = Me.translator.injectParams(sql, params)

            If Not Me.HasTransaction() Then
                Me.connection.Open()
                Using c As System.Data.IDbCommand = Me.connection.CreateCommand()
                    'no se establece transacción
                    c.CommandText = sql
                    adapter.Fill(c.ExecuteReader())
                End Using
                Me.connection.Close()
            Else
                'la conexión debería esta abierta
                Using c As System.Data.IDbCommand = Me.connection.CreateCommand()
                    c.Transaction = Me.currentTransaction
                    c.CommandText = sql
                    adapter.Fill(c.ExecuteReader())
                End Using
                'no se debe cerrar la conexión
            End If

            Return True
        End Function

        ''' <summary>
        ''' Encuentra todos los objetos del tipo T según el criterio indicado
        ''' </summary>
        ''' <param name="oql">un criterio OQL, por ejemplo: where nro like '%?1%' and edad>?2</param>
        ''' <param name="params">una lista de parámetros, que sustituirán a las variables indicadas en el criterio</param>
        ''' <returns>Una colección del tipo T indicado</returns>
        Public Function Find(ByVal oql As String, ByVal params() As Object, ByVal modelType As Type) As List(Of Model)

            Dim element = Activator.CreateInstance(modelType)
            Dim elements As New List(Of Model)
            Dim sql As String = Me.translator.getFindQuery(element, oql, params)

            Me.connection.Open()

            Using c As System.Data.IDbCommand = Me.connection.CreateCommand()
                c.CommandText = sql
                Using reader As IDataReader = c.ExecuteReader()

                    While reader.Read()

                        Dim pi As System.Reflection.PropertyInfo() = element.GetType.GetProperties()
                        For Each p As System.Reflection.PropertyInfo In pi
                            If reader(p.Name).GetType() Is GetType(System.DBNull) Then
                                p.SetValue(element, Nothing, Nothing)
                            Else
                                p.SetValue(element, reader(p.Name), Nothing)
                            End If
                        Next
                        CType(element, Model).setPersisted(True)
                        elements.Add(element)
                        element = Activator.CreateInstance(modelType)
                    End While

                End Using

            End Using

            Me.connection.Close()

            Return elements

        End Function


        Public Function getTranslator() As Translators.DataSource.SQLTranslator
            Return Me.translator
        End Function

        Public Function getState() As Data.ConnectionState
            Return Me.connection.State
        End Function

#End Region


    End Class




End Namespace