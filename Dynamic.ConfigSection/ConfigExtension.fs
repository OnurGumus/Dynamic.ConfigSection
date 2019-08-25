namespace Dynamic.ConfigSection

open System.Runtime.CompilerServices
open Microsoft.Extensions.Configuration
open System

[<Extension>]
type ConfigExtension () =
    [<Extension>]
    /// <summary>
    /// An extension method that returns given string as an dynamic Expando object
    /// </summary>
    /// <param name="configuration">configuration to be searched</param>
    /// <param name="maxSize">section to be retrieved</param>
    /// <returns>An expando object represents given section</returns>
    /// <exception cref="System.ArgumentNullException">Thrown configuration or section is null</exception>
    static member GetSectionAsDynamic(configuration : IConfiguration , section : string) :  [<return:Dynamic>] obj =
        if configuration |> isNull then "configuration" |> ArgumentNullException |> raise
        if section |> isNull then "section" |> ArgumentNullException |> raise
        let configs = configuration.AsEnumerable() |> Seq.filter (fun k-> k.Key.StartsWith(sprintf "%s:" section))
        Impl.getSection configs section

