#light

namespace FancySilverligthFSharpApp

    open System
    open System.IO
    open System.Windows
    open System.Windows.Controls

    type App() as this = 
        inherit Application()
        
        let rootVisual = AgUtil.loadXamlUserControl "Container"
        
        let contentContainer = rootVisual.FindName("subAppContent") :?> ContentControl

        let setTitle text =
            let title = rootVisual.FindName("subAppTitle") :?> TextBlock
            title.Text <- text
            
        let showDifferenceAnalysis =
            let part = new DifferenceAnalysisController(contentContainer)
            setTitle part.Title
        
        do
            AgUtil.setDocumentTitle "F#ncy F# Silverlight App"
            this.Startup.Add (fun _ -> this.RootVisual <- rootVisual)
            
            showDifferenceAnalysis


            
            

            