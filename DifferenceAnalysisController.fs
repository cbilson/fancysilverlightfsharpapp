#light

namespace FancySilverligthFSharpApp

    open System
    open System.IO
    open System.Windows
    open System.Windows.Controls
    open DifferenceAnalysis

    type DifferenceAnalysisController(container:ContentControl) as this =
        let part = AgUtil.loadXamlUserControl "DifferenceAnalysisView"
        
        let termsTextBox = part.FindName("termsTextBox") :?> TextBox
        let resultsTextBox = part.FindName("resultsTextBox") :?> TextBox
        
        let getTerms (text:string)=
            text.Split [|','|]
            |> Array.map (fun x -> x.Trim())
            |> Array.map (fun x -> match Int32.TryParse x with
                                   | (true, value) -> value
                                   | _ -> 0)
            |> List.of_array
            
        let degreeName xs =
            match DifferenceAnalysis.degree xs with
            | 0 -> "not a polynomial (degree 0)"
            | 1 -> "1st degree polynomial"
            | 2 -> "2nd degree polynomial"
            | 3 -> "3rd degree polynomial"
            | n -> sprintf "%dth degree polynomial" n
            
        let createDifferenceAnalysisReport xs difs =
            let writer = new StringWriter()
            writer.WriteLine("Analyzing sequence:")
            writer.WriteLine(String.Join(", ", (xs |> List.map (fun x -> x.ToString()) |> Array.of_list)))
            writer.WriteLine()
            writer.WriteLine("Results:")
            difs |> List.rev 
                 |> List.iter (fun xs -> let xs' = xs |> List.map (fun x -> x.ToString())
                                                      |> Array.of_list
                                         writer.WriteLine(String.Join(", ", xs')))
            writer.WriteLine()
            
            writer.WriteLine(degreeName xs)
            writer.WriteLine()

            let continued = DifferenceAnalysis.continue' xs
            let x, y, z = Seq.nth(0) continued,
                          Seq.nth(1) continued,
                          Seq.nth(2) continued  

            // NOTE: Silverlight gives a MethodAccessException when using:
            // writer.WriteLine("Next 3 elements would be: {0}, {1}, and {2}", x, y, z)
            writer.Write("Next 3 elements would be: {0}, ", x)
            writer.Write("{0}, ", y)
            writer.WriteLine("and {0}", z)
            
            writer.ToString()
            
        let runDifferenceAnalysis args =
            let report = try
                             let xs = getTerms termsTextBox.Text
                             let difs = DifferenceAnalysis.difLists [xs]
                             createDifferenceAnalysisReport xs difs
                         with
                            | Failure msg -> msg
                            | ex -> ex.ToString()
                
            resultsTextBox.Text <- report

        do
            container.Content <- part
            termsTextBox.TextChanged.Add runDifferenceAnalysis
            

        member this.Title with get() = "Difference Analysis"
