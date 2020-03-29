# MLCriticalSection
This is a project developed in **.NET Core** and **ML.NET** library for the analysis and prediction of data contention by varying various parameters, such as the number of threads, 
the size of the critical section and the time of use.

### How it works
The project is made up of three distinct modules: the simulation module for simulating data contention; the module for the creation of a prediction model 
based on machine learning through the **ML.NET** library; the orchestrator module for management and orchestration of simulation and prediction module.
The simulations are parametric and allow you to change the configurations at will. 
The system will take care of the simulations and the creation of a predictive model through machine learning.

### Architecture
![Alt text](/wiki/img/Architecture.png?raw=true)

### The Case Study
The case sudy is a didactic simulation of the data contention in a concurrent environment. The critical section is represented as a circular list of objects, a set of threads 
tries to access the critical section by scanning the list in search of a non-locked slot.

![Alt text](/wiki/img/CaseStudy.png?raw=true)

### How to configure it
The simulation and machine learning model configurations are fully described in the JSON *appsettings.json* file within the orchestrator's project.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  },
  "Simulation": {
    "MinThread": "2",
    "MaxThread": "4",
    "ThreadRampUpPeriod": "100",
    "MinTimeOnSection": "100",
    "MaxTimeOnSection": "500",
    "MinCriticalSectionDimension": "1",
    "MaxCriticalSectionDimension": "5",
    "PredictionSimulationTime": "15",
    "TotalSimulationDimension": "400",
    "PartialSimulationDimension": "10",
    "TrainingSetPercentage": "70"
  }
}
```

In the configuration file you can change any parameter of the simulation as you like, such as the thread interval, the interval of the size of the 
critical section and the interval of the time of use. It is also possible to modify the number of total records to be simulated, the training time 
of the machine learning model and the size, in percentage, of the training set.

### Usage
To start the project, simply launch the **CriticalSectionOrchestrator** project on VS after having appropriately configured the parameters for the simulation in the JSON file.

### ML.NET
ML.NET is an open source and cross-platform machine learning framework, below the references of the project:
https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet
https://github.com/dotnet/machinelearning

### Contributing
Thank you for considering to help out with the source code! We welcome contributions from anyone on the internet, and are grateful for even the smallest of fixes!
If you'd like to contribute, please fork, fix, commit and send a pull request for the maintainers to review and merge into the main code base.

**Getting started with Git and GitHub**

 * [Setting up Git for Windows and connecting to GitHub](http://help.github.com/win-set-up-git/)
 * [Forking a GitHub repository](http://help.github.com/fork-a-repo/)
 * [The simple guide to GIT guide](http://rogerdudler.github.com/git-guide/)
 * [Open an issue](https://github.com/engineering87/MLCriticalSection/issues) if you encounter a bug or have a suggestion for improvements/features

### Licensee
MLCriticalSection source code is available under MIT License, see license in the source.

### Contact
Please contact at francesco.delre.87[at]gmail.com for any details.
