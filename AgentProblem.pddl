(define (problem detective-problem)
   (:domain detective-domain)

   (:objects kitchen dining-room hall garden bedroom guest-bedroom bathroom attic
             clerk rich politician mafia-boss journalist
             judge
    )
          
   (:init (ROOM kitchen) (ROOM dining-room) (ROOM hall) (ROOM garden) (ROOM bedroom) (ROOM guest-bedroom) (ROOM bathroom) (ROOM attic)
          (AGENT clerk) (AGENT rich) (AGENT politician) (AGENT mafia-boss) (AGENT journalist)
          (KILLER judge)
          (alive clerk) (alive rich) (alive politician) (alive mafia-boss) (alive journalist) (alive judge)
          (in-room clerk hall) (in-room rich hall) (in-room politician garden) (in-room mafia-boss garden) (in-room journalist bedroom) (in-room judge hall)
          (contains-evidence garden)
          
   )
       
   (:goal (and (died judge)
          )
   )
)