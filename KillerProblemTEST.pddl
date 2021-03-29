(define (problem detective-problem)
(:domain detective-domain)
(:objects kitchen dining-room hall garden bedroom guest-bedroom bathroom attic Mafia-boss Politician Clerk Journalist Rich Judge )
(:init (ROOM kitchen) (ROOM dining-room) (ROOM hall) (ROOM garden) (ROOM bedroom) (ROOM guest-bedroom) (ROOM bathroom) (ROOM attic) (AGENT Mafia-boss) (died Mafia-boss) (in-room Mafia-boss attic) (AGENT Politician) (died Politician) (in-room Politician bedroom) (AGENT Clerk) (died Clerk) (in-room Clerk bathroom) (AGENT Journalist) (alive Journalist) (in-room Journalist hall) (AGENT Rich) (died Rich) (in-room Rich guest-bedroom) (KILLER Judge) (alive Judge) (in-room Judge hall) )
(:goal (and (died Mafia-boss) (died Politician) (died Clerk) (died Journalist) (died Rich) ))
)
