![alt text](https://raw.githubusercontent.com/alerdenisov/Rentitas/master/Resources/rentitas-logo200.png "Rentitas Modular ECS framework for Unity")
# Rentitas - modular ECS framework for Unity
Rentitas is a modular Entity Component Systems framework base on Entitas, but some changes:
- **Strong typed** (Each pool define interface (clean one) which must be implemented by components)
- **Modular** (Each part of application lives in kernels and can be loaded/unloaded at any time)
- **Type based** (To leave codegeneration and allow to use kernels as a DLL each component index based on their types)

Rentitas is in active development as a part of huge MMORPG project: Age of Guild.

## Core features
- Loading/unloading application modules (IKernel's) (Which can be DLL too!)
- Reactive (Execute systems only when data is updated)
- Simple (Everything is plain as possible. No more over inheritance and complicated archetecture)

## Documentation and Tutorials
- [API Reference](https://github.com/SmallPlanetUnity/PlanetUnity2/blob/master/Documentation/TableOfContents.md)
- [Documentation](https://github.com/SmallPlanetUnity/PlanetUnity2/blob/master/Documentation/TableOfContents.md)
- [Tutorials](https://github.com/SmallPlanetUnity/PlanetUnity2/blob/master/Documentation/TableOfContents.md)
- [Examples](https://github.com/SmallPlanetUnity/PlanetUnity2/blob/master/Documentation/TableOfContents.md)

## Roadmap
- [x] Indexes ([read more..](https://github.com))
  - [ ] Search indexes ([read more..](https://github.com))
- [ ] Smartest manage kernels ([read more..](https://github.com))
- [ ] Accessability between kernels ([read more..](https://github.com))

## Limitation and Bugs
Not found yet.. but it doesn't mean that they're not exists :D
**Found one? Tell us via [issue](https://github.com/alerdenisov/ReUI/issues/new)**

## Contributing to Rentitas
The project is hosted on GitHub where you can [report issues](https://github.com/alerdenisov/ReUI/issues), fork the project and [submit pull requests](https://github.com/alerdenisov/ReUI/pulls).

### Before start to implement feautre
[Create a new ticket](https://github.com/alerdenisov/ReUI/issues/new) to let people know what you're working on and to encourage a discussion. Follow the [git-flow](https://github.com/nvie/gitflow) conventions and create a new feature branch starting with the issue number: `git flow feature start <#issue-your-feature>`