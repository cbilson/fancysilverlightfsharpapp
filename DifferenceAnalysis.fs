#light

module DifferenceAnalysis =

    (*
        Translating some functions in the Polynomials chapter of
        "The Haskell Road to Logic Maths and Programming"
        (chapter 9)
        http://fldit-www.cs.uni-dortmund.de/~peter/PS07/HR.pdf
    *)
    let N = Seq.init_infinite (fun x -> x)
     
    let rec difs = function
        | [] -> []
        | [m] -> []
        | n :: m :: ks -> (m - n) :: difs (m :: ks)
        
    let constant = function
        | [] -> true
        | x :: xs -> List.for_all (fun y -> x = y) xs
        
    let rec difLists = function
        | (xs :: xss) as lists when constant xs -> lists
        | (xs :: xss) as lists -> difLists ((difs xs) :: lists)
        | [] -> failwith "lack of data or not a polynomial function"
     
    let last = function
        | [] -> failwith "no elements"
        | xs -> xs.[List.length xs - 1]
        
    let genDifs = function
        | xs -> List.map last (difLists [xs])
           
    let rec nextD = function
        | [] -> failwith "no data"
        | [n] -> [n]
        | n :: m :: ks -> n :: nextD (n + m :: ks)
     
    let next = genDifs >> nextD >> last
     
    let differences xs = xs |> genDifs |> nextD
     
    let rec iterate f x = seq { yield x;
                                yield! iterate f (f x) }
     
    (*
        Continue any list of polynomial form, given enough initial elements
    *)
    let rec continue' xs = differences xs
                           |> iterate nextD
                           |> Seq.map last

    let degree xs = (difLists [xs] |> List.length) - 1
