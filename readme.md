# Weekly budget (console)




## Lessons learnt

Problem:
We are making async api calls while keeping the menu usable. Output in those threads 
executing in an unexpected order which gives a large chance for bugged output. 

Instead we should use a shared state and only do output in one place.