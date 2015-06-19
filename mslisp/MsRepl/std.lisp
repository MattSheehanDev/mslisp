;;; This is the standard mslisp library,
;;; currently under the filename std.lisp
;;;
;;; NOTE: I might change the name to sml.lisp (standard macro library),
;;; once macros are implemented.


; Y = ¦Ëf.(¦Ëx.f(x x))(¦Ëx.f(x x))
(define Y
    (lambda (m)
      ((lambda (z) (z z))
       (lambda (f)
	(m (lambda (a) ((f f) a)))))))

; factorial using y-combinator.
; defines f(x).
(define fac
    (Y
     (lambda (f)
       (lambda (x)
	 (if (< x 2)
	     1
	     (* x (f (- x 1))))))))

; find length using regular recursion.
(define length
    (lambda (list)
      (begin
       (define count (lambda (listp num)
		       (if (null? listp)
			   num
			   (count (cdr listp) (inc num)))))
       (count list 0))))

; returns opposite of input.
; equivalent to the more conventional !true or !false
(define not
    (lambda (x)
      (if (null? x) #t nil)))

; checks if input is equal to nil or ()
(define null?
    (lambda (x)
      (if (equals? x nil)
	  #t
	  nil)))

; joins two lists together.
(define append
    (lambda (x y)
      (if (null? x)
	  y
	  (cons (car x) (append (cdr x) y)))))

; creates a list of two items.
(define pair
    (lambda (x y)
      (cons x (cons y '()))))

; &rest parameters are not implemented yet,
; but if they were [list] would be preferred
; over [pair].
(define list
    (lambda (&rest x)
      (if (null? x)
	  (quote ())
	  (cons (car x) (list (cdr x))))))

; x++
(define inc
    (lambda (x)
      (+ 1 x)))

; x--
(define dec
    (lambda (x)
      (- x 1)))


; eval in lisp.
; e is a lisp expression, a is the environment
; todo: create if/cond function/macro
(define eval
    (lambda (e a)
      (cond
	; atom or symbol
	; todo: check for condition type symbol
	;       seperately in next conditional.
	((atom? e)
	 (cond
	   ((null? (assoc e a)) e)
	   (#t (assoc e a))))
	; list
	((atom? (car e))
	 (cond
	   ; CAR
	   ((equals? (car e) 'car)
	    (car (eval (cadr e) a)))
	   ; CDR
	   ((equals? (car e) 'cdr)
	    (cdr (eval (cadr e) a)))
	   ; CONS
	   ((equals? (car e) 'cons)
	    (cons (eval (cadr e) a) (eval (caddr e) a)))
	   ; ATOM?
	   ((equals? (car e) 'atom?)
	    (atom? (eval (cadr e) a)))
	   ; EQUALS?
	   ((equals? (car e) 'equals?)
	    (equals? (eval (cadr e) a) (eval (caddr e) a)))
	   ; QUOTE
	   ((equals? (car e) 'quote)
	    (cadr e))
	   ; COND
	   ((equals? (car e) 'cond)
	    (evcon (cdr e) a))
	   ; assume a symbol that is defined in the environment.
	   ; replace symbol with binding
	   ; re-evaluate list
	   (#t
	    (eval (cons (assoc (car e) a) (cdr e)) a))))
	; lambda functions,
	; re-evaluate with lambda body
	; with updated environment,
	; mapping the arguments and parameters together
	((equals? (caar e) 'lambda)
	 (eval (caddar e) (append (map (cadar e) (evlis (cdr e) a)) a))))))


; we could replace if with a recursive cond,
; or replace cond with a recursive if.
; something to think about.
; evaluate conditionals
(define evcon
    (lambda (c a)
      (cond
	((eval (caar c) a)
	 (eval (cadar c) a))
	(#t
	 (evcon (cdr c) a)))))

; evaluate list
(define evlis
    (lambda  (m a)
      (cond
	((null? m)
	 nil)
	(#t
	 (cons (eval (car m) a) (evlis (cdr m) a))))))
	
; takes two lists '(1 2 3) '(4 5 6) and forms pairs,
; ((1 4) (2 5) (3 6))
(define map
    (lambda (x y)
      (if (null? x)
	  (quote ())
	  (cons (pair (car x) (car y)) (map (cdr x) (cdr y))))))

; x is a key, y is a map
; returns value
(define assoc
    (lambda (x y)
      (if (null? y)
	  nil
	  (if (equals? (caar y) x)
	      (cadar y)
	      (assoc x (cdr y))))))

; [cdr [car x]]
(define cdar
    (lambda (x)
      (cdr (car x))))

; [car [car x]]
(define caar
    (lambda (x)
      (car (car x))))

; [car [cdr x]]
(define cadr
    (lambda (x)
      (car (cdr x))))

; [car [cdr [car x]]]
(define cadar
    (lambda (x)
      (cadr (car x))))

; [car [cdr [cdr x]]]
(define caddr
    (lambda (x)
      (cadr (cdr x))))

; [car [cdr [cdr [car x]]]]
(define caddar
    (lambda (x)
      (caddr (car x))))
	  
