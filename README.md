This repository contains three projects which are individually very small.  They were originally bundled as a single
library package but as of April 2020, they have been separated into three distinct NuGet packages.

In addition, with version 7.0.0, a large amount of obsolete & redundant functionality *has either been withdrawn or moved to other repositories*.

## [CSF.Enums]
The enums package provides extension methods for `enum` objects, in the classes [`EnumDefinitionExtensions`] and
[`EnumFlagsExtensions`] - both in the `CSF` namespace.  The definition extensions are shortcuts to detect and assert
whether or not an object is a defined enum value (useful for parameter-checking). The flags extensions are used for
asserting that a given enum type is a `[Flags]`-based enum, and also a method for getting all of the current flags
values which are contained within a specified value.

[CSF.Enums]: https://www.nuget.org/packages/CSF.Enums/
[`EnumDefinitionExtensions`]: https://github.com/csf-dev/CSF.Utils/blob/v7.0.0/CSF.Enums/EnumDefinitionExtensions.cs
[`EnumFlagsExtensions`]: https://github.com/csf-dev/CSF.Utils/blob/v7.0.0/CSF.Enums/EnumFlagsExtensions.cs

## [CSF.Guids]
The GUIDs package provides two extension methods (in the `CSF` namespace) to convert a `Guid` to/from
[an RFC-4122 UUID] byte array.  The dotnet `Guid` implementation does not quite conform to this specification;
these extension methods may be used to get a conforming byte array (by re-ordering the bytes based on the
endianness of the computer).

Additionally, it provides an interface that defines a strategy by which `Guid` instances may be created.  As well
as a 'normal' implementation, there is also a [`GuidCombCreator`] class available, which creates semi-sequential
instances, using [the 'COMB' algorithm], documented [in detail on this page] of the linked article (albeit a SQL implementation).

[CSF.Guids]: https://www.nuget.org/packages/CSF.Guids/
[`GuidCombCreator`]: https://github.com/csf-dev/CSF.Utils/blob/v7.0.0/CSF.Guids/GuidCombCreator.cs
[an RFC-4122 UUID]: https://tools.ietf.org/html/rfc4122
[the 'COMB' algorithm]: https://www.informit.com/articles/article.aspx?p=25862
[in detail on this page]: https://www.informit.com/articles/article.aspx?p=25862&amp;seqNum=7

## [CSF.IO]
The I/O package provides three extension methods (in the `CSF` namespace) relating to instances of `FileSystemInfo`,
to perform common actions.  It also provides a builder object - [`FilenameExtensionBuilder`] - which may be used to
manipulate a filename, altering its filename extensions.

[CSF.IO]: https://www.nuget.org/packages/CSF.IO/
[`FilenameExtensionBuilder`]: https://github.com/csf-dev/CSF.Utils/blob/v7.0.0/CSF.IO/FilenameExtensionBuilder.cs

## Open source license
All source files within this project are released as open source software,
under the terms of [the MIT license].

[the MIT license]: http://opensource.org/licenses/MIT

This software is distributed in the hope that it will be useful, but please
remember that:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
