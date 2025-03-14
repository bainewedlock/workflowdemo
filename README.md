## Description
This can be used as a template for an app that uses
Daniel Gerlags workflow-core library
inside a windows service with a web ui for management.

## WalzWorkflow
I made generic classes which address some of our usual
requirements and put them in the folder 'WalzWorkflow'.
For a usage example please check out the folder 'Workflow_Demo'.

## Management UI
We want an overview of workflows, especially unfinished ones.
We want to see the details of a failed workflow and 
a button to easily retry at its current step.

## Assets
We want to use a folder for each workflow to store related data.
This is so you can easily save intermediate files in one step
and use them in a later step.
(for example, create a pdf in step 1 and send it via mail in step 2)

In order for this to work you have to specify a business key (BK)
as the workflows REFERENCE when starting it, like so:
```
var BK = "order.5023423499";
await host.StartWorkflow("checkoutmail", null, BK);
```

## Traceability
We want a history of what happened, stored separately for each workflow:
- meta info, like at what time the individual steps started
- custom logs
- exceptions
- user interactions (resuming / termination)

## Note about obsolete method in library
- GetWorkflowInstances is marked as obsolete
- I'm still using it is useful and there is no alternative in the API yet
- a workaround could be to bypass the API and access the DB directly
- see https://github.com/danielgerlag/workflow-core/discussions/861

## Ideas
- purging of old workflows should be scheduled
  (maybe with a BackgroundService from Microsoft.Extensions.Hosting)
