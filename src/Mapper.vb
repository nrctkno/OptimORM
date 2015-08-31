Namespace OptimOrm


    Public Class Mapper


#Region "miembros privados"
        ''' <summary>
        ''' Maps each attribute of the table as a property
        ''' </summary>
        Private Shared Sub mapTable(ByVal w As System.IO.TextWriter, ByVal parser As Translators.Code.DotNetTranslator, ByVal table As String, Optional ByVal [namespace] As String = "")

            Dim columns As List(Of Dictionary(Of String, Object))
            Dim properties As System.Text.StringBuilder 'used as a buffer to avoid scanning twice the dictionary of fields
            Dim relationDefinitions As System.Text.StringBuilder

            If ([namespace].Length > 0) Then
                w.WriteLine("Namespace " & [namespace])
                w.WriteLine()
                w.WriteLine()
            End If


            w.WriteLine("Public Class " & table)
            w.WriteLine(" inherits OptimOrm.Model")

            columns = New List(Of Dictionary(Of String, Object))
            properties = New System.Text.StringBuilder()
            relationDefinitions = New System.Text.StringBuilder()

            Connection.getInstance().executeQuery(Connection.getInstance().getTranslator().getQueryForListAllColumns(table), New Adapters.KeyValueAdapter(columns))

            w.WriteLine()

            Dim field As String
            Dim type As String
            Dim key As String = ""

            For Each ce As Dictionary(Of String, Object) In columns
                If Not IsDBNull(ce("Key")) Then
                    If Connection.getInstance().getTranslator().isKey(ce("Key")) Then key = ce("Field")
                End If

                field = ce("Field")
                type = Connection.getInstance().getTranslator().translateType(ce("Type"))
                w.WriteLine(" Private _" & field & " as " & type)
                parser.makeProperty(field, "_" & field, type, properties) : properties.AppendLine()
            Next

            w.WriteLine()
            w.WriteLine("#region ""public methods""") : w.WriteLine()
            w.WriteLine(properties.ToString())
            w.WriteLine(" Public Overrides Function PrimaryKey() As String")
            w.WriteLine("  return """ & key & """")
            w.WriteLine(" End Function") : w.WriteLine()


            'maps every parent relation 
            columns = New List(Of Dictionary(Of String, Object))

            Connection.getInstance().executeQuery(Connection.getInstance().getTranslator().getQueryForListAllParentRelationships(table), New Adapters.KeyValueAdapter(columns))

            For Each ce As Dictionary(Of String, Object) In columns
                relationDefinitions.AppendLine("  Me.AddReadOnlyRelation(""" & ce("pt") & """,""" & ce("fc") & """,GetType(" & ce("pt") & "),""" & ce("pc") & """)")
                'the getter
                w.WriteLine(" Public function get" & ce("pt") & "() as " & ce("pt"))
                w.WriteLine("  return me.getRelation(""" & ce("pt") & """).GetUnique()")
                w.WriteLine(" End function") : w.WriteLine()
                'the setter
                w.WriteLine(" Public sub set" & ce("pt") & "(ByVal m as " & ce("pt") & ")")
                w.WriteLine("  me.getRelation(""" & ce("pt") & """).AddUnique(m)")
                w.WriteLine(" End sub") : w.WriteLine()
            Next

            'maps every child relation 
            columns = New List(Of Dictionary(Of String, Object))

            Connection.getInstance().executeQuery(Connection.getInstance().getTranslator().getQueryForListAllChildrenRelationships(table), New Adapters.KeyValueAdapter(columns))

            For Each ce As Dictionary(Of String, Object) In columns
                relationDefinitions.AppendLine("  Me.AddRelation(""" & ce("ft") & """,""" & ce("pc") & """,GetType(" & ce("ft") & "),""" & ce("fc") & """)")

                w.WriteLine(" Public function get" & ce("ft") & "() As List(Of OptimOrm.Model)")
                w.WriteLine("  return me.getRelation(""" & ce("ft") & """).GetAll()")
                w.WriteLine(" End function") : w.WriteLine()

                w.WriteLine(" Public sub set" & ce("ft") & "(ByVal m as " & ce("ft") & ")")
                w.WriteLine("  me.getRelation(""" & ce("ft") & """).AddAsForeign(m)")
                w.WriteLine(" End sub") : w.WriteLine()
            Next

            w.WriteLine(" Public sub New()")
            w.Write(relationDefinitions.ToString())
            w.WriteLine(" End Sub") : w.WriteLine()

            w.WriteLine("#end region") : w.WriteLine()
            w.WriteLine("End Class")

            If ([namespace].Length > 0) Then
                w.WriteLine()
                w.WriteLine()
                w.WriteLine("End Namespace")
            End If

        End Sub

#End Region


#Region "miembros públicos"

        Public Shared Sub buildClasses(ByVal w As System.IO.TextWriter, ByVal parser As Translators.Code.DotNetTranslator, Optional ByVal [namespace] As String = "")
            Dim tables As New List(Of Dictionary(Of String, Object))
            Connection.getInstance().executeQuery(Connection.getInstance().getTranslator().getQueryForListAllTables(), New Adapters.KeyValueAdapter(tables))
            For Each te As Dictionary(Of String, Object) In tables
                Dim tkc As New Dictionary(Of String, Object).KeyCollection(te)
                For Each tk As String In tkc
                    Console.Out.WriteLine(te(""))
                    mapTable(w, parser, te(tk), [namespace])
                Next
            Next
        End Sub

        Public Shared Sub buildClasses(ByVal path As String, ByVal parser As Translators.Code.DotNetTranslator, Optional ByVal [namespace] As String = "")
            Dim tables As New List(Of Dictionary(Of String, Object))
            Connection.getInstance().executeQuery(Connection.getInstance().getTranslator().getQueryForListAllTables(), New Adapters.KeyValueAdapter(tables))
            For Each te As Dictionary(Of String, Object) In tables
                Dim tkc As New Dictionary(Of String, Object).KeyCollection(te)
                For Each tk As String In tkc
                    Dim w As New System.IO.StreamWriter(path & te(tk).ToString() & "." & parser.getFileExtension())
                    mapTable(w, parser, te(tk), [namespace])
                    w.Close()
                Next
            Next
        End Sub

#End Region


    End Class


End Namespace