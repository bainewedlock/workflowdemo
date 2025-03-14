## TODO
- maintenance page
  - archive / delete old workflows including assets
  - integrity check
    - show if there are asset folders without a workflow or vice versa
- auto-archivieren 

## Note 1
- GetWorkflowInstances is marked as obsolete
- I'm still using it is useful and there is no alternative in the API yet
- a workaround could be to bypass the API and access the DB directly
- see https://github.com/danielgerlag/workflow-core/discussions/861

## Note 2
- it seems there is no API function to cleanup old workflows 
- as a workaround I use WorkflowContext.cs (generated with EF scaffolding)

## History
- added client side library @microsoft/signalr