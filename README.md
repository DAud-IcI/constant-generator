# constant-generator
A .Net Source Generator that converts XML to C# compile time constants.

## Introduction

Sometimes you need structured constants, for example various paths or routes but manually creating those constants is error prone, a chore, or both. With this code analyser type library you can describe them as XML with minimal repetition and almost no bolerplate. You can nest fragments in a way that's easy to comprehend at a glance and the analyser automatically creates the nested static classes and string constants with the full paths glued together as values.


## Example

Say you want custom routes for your ASP.Net application, but also put it in a constants file so you can refer to them in your logic.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Management xmlns:g="https://github.com/DAud-IcI/constant-generator/">
    <g:Namespace Value="MyWebsite.Constants.Routes" />
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

The elements with the `g` namespace are configuration items, they are optional, must be direct children of the root and each must have a `Value` attribute.
- Namespace: You can specifiy the generated class's namespace. If you omit it, it will be the assembly's default namespace.
- Separator: The values in the constants are the full path from the root, with this string used as the separator. The default value is `/`.

The result will be like this:
```c-sharp
namespace MyWebsite.Constants.Routes
{
    public static class Management
    {
        public const string Users = "Management/Users";
        public const string Companies = "Management/Companies";
        
        public static class New
        {
            public const string User = "Management/New/User";
            public const string Company = "Management/New/Company";
        }
        
        public static class Unassign
        {
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
