# ffplot: F# wrapper for ggplot2

[ggplot2](http://ggplot2.org/) is a great R library for flexible visualizations,
based on compositional design of plots. Unfortunately, using it from F# can be
sometimes cumbersome. To simplify using ggplot2 from F#, I put together a
simple wrapper, ffplot.

## Requirements:
To use ggplot2 from F#, you'll need the following tools:

- [R](https://www.r-project.org/) installation with the [ggplot2](http://ggplot2.org/)
  package.
- [RProvider](http://bluemountaincapital.github.io/FSharpRProvider/) -
  F# type provider for calling R from F#.
- Optional: [FsLab](http://fslab.org/) collects all the necessary dependencies and simplifies other data science tasks in F#.

## Usage:

To use `ffplot`, simply reference the `ggplot.fs` file from your solution.
You can also add the file using the  [paket](https://fsprojects.github.io/Paket/index.html) dependency manager
by adding the following line into your `paket.dependencies` file:

```
  github evelinag/ffplot ggplot.fs
```
See [paket documentation](https://fsprojects.github.io/Paket/github-dependencies.html) for more details on referencing Github dependencies.

For creating ggplot2 plots, see `examples.fsx` with both simple and more complex examples showing how to use ffplot in F#.

![ggplot2 example](ggplot-example.png)
