# constant-generator
A .Net Source Generator that converts XML to C# compile time constants.

## Introduction

Sometimes you need structured constants, for example various paths or routes but manually creating those constants is error prone, a chore, or both. With this code analyser type library you can describe them as XML with minimal repetition and almost no bolerplate. You can nest fragments in a way that's easy to comprehend at a glance and the analyser automatically creates the nested static classes and string constants with the full paths glued together as values.


## Usage

Include this project using ProjectReference with the attributes you see below. Also Include your files into the AdditionalFiles item group:

```xml
    <ItemGroup>
        <ProjectReference Include="..\ConstantGenerator\ConstantGenerator.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
        <AdditionalFiles Include="*.constants.xml" />
    </ItemGroup>
```

## Example

Say you want custom routes for your ASP.Net application, but also put it in a constants file so you can refer to them in your logic.

_sample.routes.constants.xml_
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Management xmlns:g="https://github.com/DAud-IcI/constant-generator/">
    <!-- Without this, your class will be in the Constants namespace. That can work if the file names are unique. -->
    <g:Namespace Value="ConstantGenerator.Sample.Constants.Routes" />
    
    <!-- This is the default already so you can safely omit it. -->
    <g:Separator Value="/" />

    <Users />
    <Companies />
    <New>
        <User />
        <Company />
    </New>
    <Unassign>
        <User />
    </Unassign>
</Management>
```

The elements with the `g` namespace are optional configuration items, they are optional, must be direct children of the root and each must have a `Value` attribute.
- Namespace: You can specifiy the generated class's namespace. If you omit it, it will use `Constants`.
- Separator: The values in the constants are the full path from the root, with this string used as the separator. The default value is `/`.

It generates this class:

_SampleRoutes.GeneratedConstant.cs_
```c-sharp
namespace ConstantGenerator.Sample.Constants.Routes
{
    public static class Management
    {
        public const string ThisRoute = "Management";

        public const string Users = "Management/Users";
        public const string Companies = "Management/Companies";

        public static class New
        {
            public const string ThisRoute = "Management/New";

            public const string User = "Management/New/User";
            public const string Company = "Management/New/Company";
        }

        public static class Unassign
        {
            public const string ThisRoute = "Management/Unassign";

            public const string User = "Management/Unassign/User";
        }
    }
}
```

## This seems pretty simple, so why?

For one it saves a lot of lines and boilerplate on more complex structures. Also if you did this manually you probably should have auxiliary variables like this:


```c-sharp
        public static class New
        {
            private const string NewPathBase = nameof(Management) + "/" + nameof(New);
            public const string User = NewPathBase + "/" + nameof(User);
            public const string Company = NewPathBase + "/" + nameof(Company);
        }
```

Don't get me wrong, `nameof` is great but just look at all this visual noise! We don't need that with the source generator beucase the name is generated from the same text as the value.


# Future Plans & Contributing

This project is my attempt to learn .Net source generators while making something I can actually see myself using. I plan to get out a NuGet release after the code is cleaned up, but outside of that I don't see much in way of future improvement. Of course if you have any suggestions, pull requests are welcome.
