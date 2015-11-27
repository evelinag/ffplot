#I "packages/Deedle/lib/net40"
#I "packages/Deedle.RPlugin/lib/net40"
#I "packages/RProvider/lib/net40"
#I "packages/R.NET.Community/lib/net40"
#I "packages/R.NET.Community.FSharp/lib/net40"

#r "RProvider.Runtime.dll"
#r "RProvider.dll"
#r "RDotNet.dll"
#r "RDotNet.NativeLibrary.dll"
#r "RDotNet.FSharp.dll"
#r "Deedle.dll"
#r "Deedle.RProvider.Plugin.dll"

#load "ggplot.fs"

open Deedle
open RProvider
open RProvider.ggplot2
open RProvider.datasets
open ggplot

fsi.AddPrinter(fun (synexpr:RDotNet.SymbolicExpression) ->
    synexpr.Print())

(**
Using ggplot2 from F# with RProvider
====================================================
This file contains various examples of using ggplot in F#, with
the ffplot wrapper. 

First we load some example datasets:
*)

// mtcars dataset
let mtc = R.mtcars.GetValue<Frame<string, string>>()

// Iris dataset
let iris = R.iris.GetValue<Frame<string, string>>()

// Diamonds dataset
let diamonds = R.diamonds.GetValue<Frame<string,string>>()  

//-------------------------------------------------
//   Scatter plots
//-------------------------------------------------

G.ggplot(mtc, R.aes__string(x="disp", y="drat"))
++ R.geom__point()

G.ggplot(mtc, G.aes(x="disp", y="drat"))
++ R.geom__point()


// 3 ways to create the same plot using different ggplot initialisations
G.ggplot(iris, G.aes("Sepal.Width", "Sepal.Length"))
++ R.geom__point()

G.ggplot()
++ R.geom__point(data=iris, mapping=G.aes("Sepal.Width", "Sepal.Length"))

G.ggplot(data=iris)
++ R.geom__point(G.aes("Sepal.Width", "Sepal.Length"))

//-------------------------------------------------
//   Bar plots
//-------------------------------------------------

G.ggplot(mtc, R.aes(R.factor(mtc?cyl)))
++ R.geom__bar()
++ R.coord__flip()

G.ggplot(diamonds, G.aes("clarity", fill="cut"))
++ R.geom__bar()

// Faceting
G.ggplot(diamonds, G.aes("clarity"))
++ R.geom__bar()
++ R.facet__wrap(R.eval(R.parse(text="~cut")))

//-------------------------------------------------
//   Line plots
//-------------------------------------------------

// Calling ffplot with a manually created R data frame
let x = [0.0 .. 0.1 .. 10.0]
let y = x |> List.map (fun value -> sin(value))
let dataframe = 
    namedParams ["X", x; "Value", y] 
    |> R.data_frame

G.ggplot(dataframe, G.aes(x="X", y="Value"))
++ R.geom__line()

G.ggplot(diamonds, G.aes(x="carat", y="price"))
++ R.geom__point()
++ R.geom__smooth()

G.ggplot(mtc, G.aes("mpg", "wt"))
++ R.geom__point()
++ R.geom__smooth()

//-------------------------------------------------
//   Density plots
//-------------------------------------------------

G.ggplot(diamonds, G.aes("carat"))
++ R.geom__density()

G.ggplot(diamonds, G.aes(x="carat", colour="color", fill="color")) 
++ R.geom__density()

//-------------------------------------------------
//   Histograms
//-------------------------------------------------

G.ggplot(diamonds, G.aes("carat"))
++ R.geom__histogram()

G.ggplot(diamonds, G.aes("carat", fill="color"))
++ R.geom__histogram(namedParams["binwidth", 0.1])

//-------------------------------------------------
//   Working with categorical values
//-------------------------------------------------

G.ggplot(diamonds, G.aes(x="color", y="price/carat"))
++ R.geom__point()

G.ggplot(diamonds, G.aes(x="color", y="price/carat"))
++ R.geom__jitter()

//-------------------------------------------------
//   Using more complex aesthetics
//-------------------------------------------------

// use R.aes__string function 

G.ggplot(diamonds, G.aes(x="color", y="price/carat"))
++ R.geom__boxplot(R.aes__string(namedParams["fill", "color"]))

//-------------------------------------------------
//   Composing more complex plots
//-------------------------------------------------

// change sizes of axes labels and legends
let sizeSettings () =
    R.theme(namedParams["axis.text", R.element__text(namedParams["size", 12])])
    ++ R.theme(namedParams["legend.text", R.element__text(namedParams["size", 12])])
    ++ R.theme(namedParams["axis.title", R.element__text(namedParams["size", 14])])
    ++ R.theme(namedParams["plot.title", R.element__text(namedParams["size", 18])])

// Create a plot
G.ggplot(iris, G.aes(x="Sepal.Length", y="Sepal.Width",colour="Petal.Length"))
++ R.geom__point(namedParams["size", 4])
++ R.theme__bw()
++ R.scale__color__gradient(
    namedParams["low", "blue"; "high", "gold"])
++ R.ggtitle("Iris dataset")
++ R.xlab("Sepal length")
++ R.ylab("Sepal width")
++ sizeSettings()

