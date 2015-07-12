;;; This is the standard mslisp library,
;;; currently under the filename std.lisp
;;;
;;; NOTE: I might change the name to sml.lisp (standard macro library),
;;; once macros are implemented.
;;;


;; global variables
(define nil ())
(define #f ())

;; macro to make it easier to define macros
(define defmacro
    (macro
     (lambda (name params body)
       `(define ,name
	    (macro
	     (lambda ,params
	       ,body))))))

; flattens a list, ex: (1 2 (3 4)) => (1 2 3 4)
(define flatten 
    (lambda (list)
      (begin
       (define flat 
	   (lambda (x y)
	     (if (atom? x)
		 (if (null? x)
		     y
		     (cons x y))
		 (flat (car x) (flat (cdr x) y)))))
       (flat list ()))))

;; macro to make it easier to define functions
(defmacro defun (name params body)
  `(define ,name
       (lambda ,params
	  ,body)))

; checks if equal to typeof "atom"
(defun atom? (x)
  (if (list? x)
      ()
      #t))

; checks if equal to typeof "number"
(defun number? (x)
  (if (eq? (typeof x) "number")
      #t
      ()))

; checks if arg is equal to "list"
(defun list? (x)
  (if (eq? (typeof x) "list")
      #t
      ()))

; checks if input is equal to ()
(defun null? (x)
  (if (eq? x nil)
      #t
      ()))

; checks if arg is equal to 0
(defun zero? (x)
  (if (= x 0)
      #t
      ()))

; returns opposite of input.
; equivalent to the more conventional !true or !false
(defun not (x)
  (if (null? x) #t nil))

; checks if both arguments are not nil
(defun and (x y)
  (cond
    ((null? x) ())
    ((null? y) ())
    (#t #t)))

; checks if either argument is not nil
(defun or (x y)
  (cond
    ((not (null? x)) #t)
    ((not (null? y)) #t)
    (#t ())))

(defun equan? (a b)
  (cond
    ((and (number? a) (number? b))
     (= a b))
    ((or (number? a) (number? b))
     nil)
    (#t (eq? a b))))

(defun eqlist? (a b)
  (cond
    ((and (null? a) (null? b)) 
     #t)
    ((or (null? a) (null? b)) 
     nil)
    ((and (atom? (car a)) (atom? (car b)))
     (and (equan? (car a) (car b)) (eqlist? (cdr a) (cdr b))))
    ((or (atom? (car a)) (atom? (car b))) 
     nil)
    (#t 
     (and (eqlist? (car a) (car b)) (eqlist? (cdr a) (cdr b))))))

(defun equal? (a b)
  (cond
    ((and (atom? a) (atom? b))
     (equan? a b))
    ((or (atom? a) (atom? b))
     nil)
    (#t
     (eqlist? a b))))


(defun loop (list cb)
  (cond
    ((null? (car list)) ())
    (#t (begin
	 (loop (cdr list) cb)
	 (cb (car list))))))


(defun loop-sum (list cb)
  (begin
   (let (sum ())
     (loop list (lambda (part)
		  (set! sum (cons (cb part) sum)))))
   sum))


;; + operator macro
;; creates an list like (add (add 1 2) 3)
;; to add from left to right.
;; note: length and inc must use the primitive [add] function
;;       to not get caught in infinite loop.
(defmacro + (&rest x)
  ((Y
    (lambda (f)
      (lambda (x)
	(if (= (length x) 1)
	    (car x)
	    (f (cons `(add ,@(first-pair x)) (cddr x)))))))
   x))

(defmacro * (&rest x)
  ((Y
    (lambda (f)
      (lambda (x)
	(if (= (length x) 1)
	    (car x)
	    (f (cons `(multiply ,@(first-pair x)) (cddr x)))))))
   x))

(defmacro - (&rest x)
  ((Y
    (lambda (f)
      (lambda (x)
	(if (= (length x) 1)
	    (if (number? (car x))
		(subtract 0 (car x))
		(car x))
	    (f (cons `(subtract ,@(first-pair x)) (cddr x)))))))
   x))

(defmacro / (&rest x)
  ((Y
    (lambda (f)
      (lambda (x)
	(if (= (length x) 1)
	    (if (number? (car x))
		(divide 1 (car x))
		(car x))
	    (f (cons `(divide ,@(first-pair x)) (cddr x)))))))
   x))

(defun first-pair (x)
  (cond
    ((or (null? (car x)) (null? (cadr x))) ())
    (#t (pair (car x) (cadr x)))))


;; wrap compare operators for multiple parameters.
;; name => function name.
;; func => lambda callback that takes two parameters and
;;         returns either true or false
(defmacro defcompare (name func)
  `(defun ,name (&rest x)
     ((Y
       (lambda (f)
	 (lambda (x)
	   (cond
	     ((not-greater (length x) 1) #t)
	     ((null? (,func (car x) (cadr x))) ,())
	     (#t (f (cdr x)))))))
      x)))

(defcompare >
    (lambda (x y)
      (if (greater x y)
	  #t
	  ())))

(defcompare <
    (lambda (x y)
      (if (lesser x y)
	  #t
	  ())))

(defcompare >=
    (lambda (x y)
      (if (not-lesser x y)
	  #t
	  ())))

(defcompare <=
    (lambda (x y)
      (if (not-greater x y)
	  #t
	  ())))

(defcompare =
    (lambda (x y)
      (if (equal x y)
	  #t
	  ())))

;; macro for cond
;; works as nested if statements
(defmacro cond (&rest conditions)
  `(let (else #t)
     ,((Y
	(lambda (f)
	  (lambda (x)
	    (if (null? x)
		()
		`(if (not (null? ,(caar `,x)))
		     ,(cadar `,x)
		     ,(f (cdr x)))))))
;		     ,(if (not (null? (cdr x)))
;			  (f (cdr x))
;			  ()))))))
       conditions)))

(defmacro unless (truthy exp)
  `(if ,truthy
      #t
      ,exp))

	        
(defmacro let (var-value  body)
  `((lambda (,(car var-value))
      ,body) ,(cadr var-value)))

; Y = ¦Ëf.(¦Ëx.f(x x))(¦Ëx.f(x x))
(defun Y (m)
  ((lambda (z) (z z))
   (lambda (f)
     (m (lambda (a) ((f f) a))))))

; factorial using y-combinator.
; defines f(x).
(define fac (Y (lambda (f)
		 (lambda (x)
		   (if (< x 2)
		       1
		       (* x (f (- x 1))))))))

; find the length of a list
(defun length (list)
  (cond
    ((null? list) 0)
    (else (inc (length (cdr list))))))

; x++
(defun inc (x)
  (add 1 x))

; x--
(defun dec (x)
  (subtract x 1))

; joins two lists together.
(defun append (x y)
  (if (null? x)
      y
      (cons (car x) (append (cdr x) y))))

; creates a list of two items.
(defun pair (x y)
  (cons x (cons y '())))

; &rest parameters are not implemented yet,
; but if they were [list] would be preferred
; over [pair].
(defun list (&rest x)
  (append x ()))


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
	   ((equal? (car e) 'car)
	    (car (eval (cadr e) a)))
	   ; CDR
	   ((equal? (car e) 'cdr)
	    (cdr (eval (cadr e) a)))
	   ; CONS
	   ((equal? (car e) 'cons)
	    (cons (eval (cadr e) a) (eval (caddr e) a)))
	   ; ATOM?
	   ((equal? (car e) 'atom?)
	    (atom? (eval (cadr e) a)))
	   ; EQUAL?
	   ((equal? (car e) 'equal?)
	    (equal? (eval (cadr e) a) (eval (caddr e) a)))
	   ; QUOTE
	   ((equal? (car e) 'quote)
	    (cadr e))
	   ; COND
	   ((equal? (car e) 'cond)
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
	((equal? (caar e) 'lambda)
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
	  (if (equal? (caar y) x)
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

; [cdr [cdr x]]
(define cddr
    (lambda (x)
      (cdr (cdr x))))

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

(defun nth (list pos)
  (cond
    ((null? list) ())
    ((zero? pos) (car list))
    (#t (nth (cdr list) (dec pos)))))

(defun last (list)
  (cond
    ((null? list) ())
    ((= (length list) 1) (car list))
    (#t (last (cdr list)))))

;; (defun last (list)
;;   ((Y
;;     (lambda (f)
;;       (lambda (x)
;; 	(if (= 1 (length x))
;; 	    (car x)
;; 	    (f (cdr x))))))
;;    list))
