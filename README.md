OptimORM
========

Micro ORM for .net 2.0+ applications


Integration
----

Copy the whole OptimORM folder into your project. No mappings, no XMLs, no EntityFramework.


Configuration
----

OptimOrm.Connection.Configure(New SqlClient.SqlConnection("your\_connection\_string"), New OptimOrm.Translators.DataSource._YourDatabaseTranslator_)

Available Translators:

    SQLTranslator
      ANSI92Translator
        MSSQLTranslator
        MySQLTranslator
    

Automapping
----

OptimOrm.Mapper.buildClasses("Any\Path", New OptimOrm.Translators.Code._ATranslator()_, "_Optionally, A Namespace_")

All classes inherits from _OptimOrm.Model_.

Available translators:

    DotNetTranslator
      VBTranslator


Copy the mapped classes into your application.


Usage
----

  Create an entity:

    dim a as Entity1= new Entity1()
    dim b as Entity2= new Entity2()
    a.fieldA=aValue
    a.fieldN=aValue
    b.fieldA=aValue
    b.fieldN=aValue
    b.set_ARelatedObject_(b)
    if a.save() then
      'everythig's ok
    else
      'error
    end if

Both a and b will be saved.

  ---
  
  Load an entity:

    dim a as Entity1 = OptimOrm.load(of Entity1)("yourPrimaryKeyValue")
    'a and its dependencies are now loaded, you can use it
    

About loading: _load_ method will load the requested entity and its direct dependencies. If a depends of b and b has also its dependencies, you must invoke _b.FillRelations()_ before use them.

  ---

  Query:

    dim adapter as _anyAdapter_= new OptimOrm.Adapters._anyAdapter_(_adapterInfo_)
    OptimOrm.Connection.getInstance().executeQuery("any sql statement", adapter)
    'use the adapter, if necessary

  Available adapters:

    Adapter
      ComboBoxAdapter
      DictionaryAdapter
      InfoAdapter
      ListViewAdapter
      ModelAdapter
      NullAdapter
      StreamAdapter
      StringAdapter

  
  Managed Query:

    Dim vars() As Object = {"aPattern", 5}
    dim entities as list(of Entity1) = OptimOrm.Connection.getInstance().find("where field_1 like '%?1%' and field_2>?2", vars, GetType(Entity1))
  
  ---
  
  Debugging:

    'assuming a is an instance of Model
    a.dump(0,Console.out) '
    
