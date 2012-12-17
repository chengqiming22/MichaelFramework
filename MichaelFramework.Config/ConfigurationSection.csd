<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="d0ed9acb-0435-4532-afdd-b5115bc4d562" namespace="MichaelFramework.Config" xmlSchemaNamespace="urn:xmlns:headline.tv:agent:configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
    <externalType name="Guid" namespace="System" />
    <externalType name="Rectangle" namespace="System.Drawing" />
    <enumeratedType name="eStyle" namespace="DevComponents.DotNetBar" codeGenOptions="None" />
    <externalType name="Color" namespace="System.Drawing" />
    <enumeratedType name="FormWindowState" namespace="System.Windows.Forms" codeGenOptions="None" />
    <externalType name="PaneState" namespace="MichaelFramework.Utils" />
    <enumeratedType name="eModelType" namespace="MichaelFramework.Model">
      <literals>
        <enumerationLiteral name="Type" value="0" />
        <enumerationLiteral name="Assembly" value="1" />
      </literals>
    </enumeratedType>
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="CustomSettings" codeGenOptions="Singleton, XmlnsProperty, Protection" xmlSectionName="customSettings">
      <elementProperties>
        <elementProperty name="System" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="system" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/System" />
          </type>
        </elementProperty>
        <elementProperty name="Application" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="application" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/App" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="Users" xmlItemName="singleUser" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/SingleUser" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="SingleUser">
      <attributeProperties>
        <attributeProperty name="Id" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="id" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Guid" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="MainForm" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="mainForm" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/MainForm" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="App">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="name" isReadOnly="false" defaultValue="&quot;MichaelFramework&quot;">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="AllowMultiStartup" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="allowMultiStartup" isReadOnly="false" defaultValue="&quot;True&quot;">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="RunCount" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="runCount" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="LoginForm" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="loginForm" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/LoginForm" />
          </type>
        </elementProperty>
        <elementProperty name="MainForm" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="mainForm" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/MainForm" />
          </type>
        </elementProperty>
        <elementProperty name="Users" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="users" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Users" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="LoginForm">
      <attributeProperties>
        <attributeProperty name="AssemblyQualifiedName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="assemblyQualifiedName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="AccessedUsers" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="accessedUsers" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Title" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="title" isReadOnly="false" defaultValue="&quot;登录&quot;">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="MainForm">
      <attributeProperties>
        <attributeProperty name="AssemblyQualifiedName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="assemblyQualifiedName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Bounds" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="bounds" isReadOnly="false" defaultValue="&quot;0,0,800,600&quot;">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Rectangle" />
          </type>
        </attributeProperty>
        <attributeProperty name="Style" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="style" isReadOnly="false">
          <type>
            <enumeratedTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/eStyle" />
          </type>
        </attributeProperty>
        <attributeProperty name="ColorTint" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="colorTint" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Color" />
          </type>
        </attributeProperty>
        <attributeProperty name="WindowState" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="windowState" isReadOnly="false">
          <type>
            <enumeratedTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/FormWindowState" />
          </type>
        </attributeProperty>
        <attributeProperty name="LeftPaneState" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="leftPaneState" isReadOnly="false" typeConverter="Custom" defaultValue="&quot;Visible=True,Expanded=True,WidthOrHeight=180&quot;">
          <customTypeConverter>
            <converterMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/PaneStateTypeConverter" />
          </customTypeConverter>
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/PaneState" />
          </type>
        </attributeProperty>
        <attributeProperty name="RightPaneState" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="rightPaneState" isReadOnly="false" typeConverter="Custom" defaultValue="&quot;Visible=False,Expanded=True,WidthOrHeight=180&quot;">
          <customTypeConverter>
            <converterMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/PaneStateTypeConverter" />
          </customTypeConverter>
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/PaneState" />
          </type>
        </attributeProperty>
        <attributeProperty name="SelectedRibbonTabIndex" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="selectedRibbonTabIndex" isReadOnly="false" defaultValue="0">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="BottomPaneState" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="bottomPaneState" isReadOnly="false" typeConverter="Custom" defaultValue="&quot;Visible=False,Expanded=True,WidthOrHeight=180&quot;">
          <customTypeConverter>
            <converterMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/PaneStateTypeConverter" />
          </customTypeConverter>
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/PaneState" />
          </type>
        </attributeProperty>
        <attributeProperty name="RibbonControlExpanded" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="ribbonControlExpanded" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="System">
      <attributeProperties>
        <attributeProperty name="UsePermissionControl" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="usePermissionControl" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="DataDirectory" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="dataDirectory" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="CustomInit" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="customInit" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/CustomInit" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="CustomInit" xmlItemName="customInitItem" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/CustomInitItem" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="CustomInitItem">
      <attributeProperties>
        <attributeProperty name="Key" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="key" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Method" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="method" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators>
      <stringValidator name="TestStringValidator" maxLength="255" />
    </validators>
  </propertyValidators>
  <customTypeConverters>
    <converter name="PaneStateTypeConverter">
      <type>
        <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/PaneState" />
      </type>
    </converter>
  </customTypeConverters>
</configurationSectionModel>