;;; This is the standard mslisp library,
;;; currently under the filename std.lisp
;;;
;;; NOTE: I might change the name to sml.lisp (standard macro library),
;;; once macros are implemented.

; returns opposite of input.
; equivalent to the more conventional !true or !false
(define not
    (lambda (x)
      (if (null? x) #t nil)))

; joins two lists together.
(define append
    (lambda (x y)
      (if (null? x)
	  y
	  (cons (car x) (append (cdr x) y)))))

; creates a list of two items.
(define pair
    (lambda (x y)
      (cons x (cons y (quote ())))))

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
      (if (equals? (caar y) x)
	  (cadar y)
	  (assoc x (cdr y)))))

; &rest parameters are not implemented yet,
; but if they were [list] would be preferred
; over [pair].
(define list
    (lambda (&rest x)
      (if (null? x)
	  (quote ())
	  (cons (car x) (list (cdr x))))))

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

(define inc
    (lambda (x)
      (+ 1 x)))

(define dec
    (lambda (x)
      (- x 1)))

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
	  
