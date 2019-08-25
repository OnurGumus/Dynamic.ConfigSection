module internal Impl

open System.Dynamic
open System.Collections.Generic
open Microsoft.Extensions.Configuration
open System

    let rec replaceWithArray ( parent : ExpandoObject) (key : string)  (input : ExpandoObject option) =
          match input with
          | None -> ()
          | Some input ->
              let dict = input :> IDictionary<_,_>
              let keys = dict.Keys |> List.ofSeq
              if keys |> Seq.forall(fun k -> k |> Int32.TryParse  |> fst) then
                  let arr = keys.Length |> Array.zeroCreate
                  for kvp in dict do
                      arr.[kvp.Key |> Int32.Parse] <- kvp.Value

                  let parentDict = parent :> IDictionary<_,_>
                  parentDict.Remove key |> ignore
                  parentDict.Add(key,arr)
              else
                  for childKey in  keys do
                      let newInput =
                          match dict.[childKey] with
                          | :? ExpandoObject as e -> Some e
                          | _ -> None
                      replaceWithArray input childKey newInput

      let getSection (configs : KeyValuePair<string,_> seq) section : obj =
          let result = ExpandoObject()
          for kvp in configs do
              let mutable parent = result :> IDictionary<_,_>
              let path = kvp.Key.Split(':')
              let mutable i = 0
              while i < path.Length - 1 do
                  if parent.ContainsKey(path.[i]) |> not then
                      parent.Add(path.[i] , new ExpandoObject())

                  parent <- downcast parent.[path.[i]]
                  i <- i + 1
              if kvp.Value |> isNull |> not then
                  parent.Add(path.[i], kvp.Value)
          replaceWithArray null null (Some result)
          upcast result
