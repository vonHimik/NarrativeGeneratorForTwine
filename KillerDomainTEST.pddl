(define (domain detective-domain)

   (:predicates (ROOM ?x) (AGENT ?x) (KILLER ?x)
                (alive ?x) (died ?x)
                (in-room ?x ?y)
   )
   
             
(:action killer_move :parameters (?k ?room-from ?room-to)
:precondition (and (ROOM ?room-from) (ROOM ?room-to) (KILLER ?k)
                   (alive ?k)
                   (in-room ?k ?room-from)
                   (not (died ?k)) (not (in-room ?k ?room-to)))
:effect (and (in-room ?k ?room-to)
             (not (in-room ?k ?room-from))))
             
             
(:action Entrap :parameters (?k ?a ?place)
:precondition (and (ROOM ?place) (KILLER ?k) (AGENT ?a)
                   (alive ?k) (alive ?a)
                   (in-room ?k ?place) (not (in-room ?a ?place))
               )
:effect (and (in-room ?a ?place)
         )
)
  
(:action TellAboutASuspicious :parameters (?k ?a ?place ?suspicious-place)
:precondition (and (ROOM ?place) (ROOM ?suspicious-place) (KILLER ?k) (AGENT ?a)
                   (alive ?k) (alive ?a)
                   (in-room ?k ?place) (in-room ?a ?place)
                   (not (= ?place ?suspicious-place))
               )
:effect (and (in-room ?a ?suspicious-place)
         )
)

(:action Kill :parameters (?k ?victim ?r ?a1 ?a2 ?a3 ?a4)
:precondition (and (ROOM ?r) (KILLER ?k) (AGENT ?victim) (AGENT ?a1) (AGENT ?a2) (AGENT ?a3) (AGENT ?a4)
                   (alive ?k) (alive ?victim)
                   (in-room ?k ?r) (in-room ?victim ?r)
                   (not (in-room ?a1 ?r)) (not (in-room ?a2 ?r)) (not (in-room ?a3 ?r)) (not (in-room ?a4 ?r))
                   (not (= ?a1 ?a2)) (not (= ?a1 ?a3)) (not (= ?a1 ?a4)) (not (= ?a2 ?a3)) (not (= ?a2 ?a4)) (not (= ?a3 ?a4))
              )
                   
:effect (and (died ?victim)
             (not (alive ?victim)))
)

)