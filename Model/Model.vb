Namespace OptimOrm


    Public MustInherit Class Model


#Region "miembros abstractos"

        Public MustOverride Function PrimaryKey() As String

#End Region


#Region "miembros privados"

        'Private dirty As Boolean = True 'no usada... aún...
        Private relations As Dictionary(Of String, IRelation)
        Private persisted As Boolean = False


        Private Function SaveInternal() As Boolean
            Dim c As Connection = Connection.getInstance()
            Dim s As String = ""

            Dim autocommit As Boolean = Not c.HasTransaction()

            If autocommit Then c.BeginTransaction() 'no hay transacción? crearla!

            If Me.isPersisted() Then s = c.getTranslator().getUpdateQuery(Me) Else s = c.getTranslator().getInsertQuery(Me)

            If c.executeNonQuery(s) > 0 Then
                Dim kv As KeyValuePair(Of String, IRelation)
                Me.persisted = True
                For Each kv In Me.relations
                    If Not kv.Value.SaveAll() Then
                        If autocommit Then c.RollbackTransaction()
                        Return False
                    End If
                Next
                If autocommit Then c.CommitTransaction() 'si se creó transacción acá, finalizarla
                Return True
            Else
                If autocommit Then c.RollbackTransaction()
                Return False
            End If

        End Function

        Private Function DeleteInternal() As Boolean
            If Me.isPersisted() Then 'porque no tiene sentido eliminar algo que no existe en la bd

                Dim c As Connection = Connection.getInstance()
                Dim s As String = ""
                Dim localTransaction As Boolean = Not c.hasTransaction()

                If localTransaction Then c.BeginTransaction() 'no hay transacción? crearla!

                s = c.getTranslator().getDeleteQuery(Me, Me.GetType().GetProperty(Me.PrimaryKey).GetValue(Me, Nothing))

                If c.executeNonQuery(s) > 0 Then
                    Dim kv As KeyValuePair(Of String, IRelation)
                    Me.persisted = True
                    For Each kv In Me.relations
                        If Not kv.Value.DeleteAll() Then
                            If localTransaction Then c.RollbackTransaction()
                            Return False
                        End If
                    Next
                    If localTransaction Then c.CommitTransaction() 'si se creó transacción acá, finalizarla
                    Return True
                Else
                    If localTransaction Then c.RollbackTransaction()
                    Return False
                End If
            Else
                Return True 'no hubo cambios, pero la operación fue exitosa
            End If
        End Function
#End Region


#Region "miembros protegidos"

        Protected Function getRelationKeys() As String()
            Dim keys As New List(Of String)(Me.relations.Keys)
            Return keys.ToArray()
        End Function

        ''' <summary>Devuelve una relación, indicada por una clave</summary>
        ''' <param name="key">La clave que identifica a la relación. Se distinguen mayúsculas de minúsculas.</param>
        ''' <returns>Una relación</returns>
        Protected Function getRelation(ByVal key As String) As IRelation
            Return Me.relations(key)
        End Function

        ''' <summary>Agrega una nueva relación al modelo</summary>
        ''' <param name="key">La clave con la que se accederá posteriormente a la relación. </param>
        ''' <param name="lk">el nombre del campo local que es clave foránea de remote</param>
        ''' <param name="referenced">el tipo que define al/a los modelo/s referenciados</param>
        ''' <param name="rk">el nombre del campo que es clave en el/los modelo/s referenciados</param>
        Protected Sub addRelation(ByVal key As String, ByVal lk As Object, ByVal referenced As Type, ByVal rk As String)
            Me.relations.Add(key, New Relation(Me, lk, referenced, rk))
        End Sub

        ''' <summary>Agrega una nueva relación de sólo lectura. En dicha relación, la entidad referenciada no será creada al guardar la entidad primaria</summary>
        ''' <param name="key">La clave con la que se accederá posteriormente a la relación. </param>
        ''' <param name="lk">el nombre del campo local que es clave foránea de remote</param>
        ''' <param name="referenced">el tipo que define al/a los modelo/s referenciados</param>
        ''' <param name="rk">el nombre del campo que es clave en el/los modelo/s referenciados</param>
        Protected Sub addReadOnlyRelation(ByVal key As String, ByVal lk As Object, ByVal referenced As Type, ByVal rk As String)
            Me.relations.Add(key, New ReadOnlyRelation(Me, lk, referenced, rk))
        End Sub

#End Region


#Region "miembros públicos"

        Public Event OnSave()
        Public Event OnDelete()


        Public Sub New()
            Me.relations = New Dictionary(Of String, IRelation)
        End Sub

        Public Overridable Function TableName() As String
            Return MyClass.GetType().Name
        End Function

        Public Function isPersisted() As Boolean
            Return Me.persisted
        End Function

        Public Sub setPersisted(ByVal p As Boolean)
            Me.persisted = p
        End Sub

        Public Function Save() As Boolean
            Dim ok As Boolean = Me.SaveInternal()
            If ok Then RaiseEvent OnSave()
            Return ok
        End Function

        Public Function Delete() As Boolean
            Dim ok As Boolean = Me.DeleteInternal()
            If ok Then RaiseEvent OnDelete()
            Return ok
        End Function

        Public Shared Function Load(Of T As {Model, New})(ByVal pk As Object) As T
            Dim c As Connection = Connection.getInstance()
            Dim obj As T = Activator.CreateInstance(GetType(T))
            Dim s As String = c.getTranslator().getLoadQuery(obj, pk)
            If c.executeQuery(s, New Adapters.ModelAdapter(obj)) Then
                obj.persisted = True
                obj.FillRelations()
                Return obj
            Else
                Return Nothing
            End If
        End Function

        Public Sub FillRelations()
            Dim kv As KeyValuePair(Of String, IRelation)
            For Each kv In Me.relations
                kv.Value.Fill()
            Next
        End Sub

        ''' <summary>Vuelca el contenido del modelo</summary>
        ''' <param name="nestingLevel">Un valor que determina la identación inicial. Recomendado: 0.</param>
        ''' <param name="w">El flujo de texto en el que se volcará el contenido</param>
        Public Sub dump(ByVal nestingLevel As Integer, ByVal w As System.IO.TextWriter)
            w.WriteLine(Space(nestingLevel) & "Model " & Me.GetType().Name)
            nestingLevel += 2
            For Each p As System.Reflection.PropertyInfo In Me.GetType.GetProperties()
                w.Write(Space(nestingLevel) & p.Name & ": ")
                If (p.GetValue(Me, Nothing)) Is Nothing Then w.Write("null") Else w.Write(p.GetValue(Me, Nothing).ToString())
                w.WriteLine()
            Next
            Dim kv As KeyValuePair(Of String, IRelation)
            For Each kv In Me.relations
                kv.Value.dump(nestingLevel, w)
            Next
        End Sub

#End Region


    End Class


End Namespace