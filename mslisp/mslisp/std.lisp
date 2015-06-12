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

; the following to procedures could be combined
; with a [let] macro.
(define length
    (lambda (list)
      (count-length list 0)))

(define count-length
    (lambda (list count)
      (if (null? list)
	  count
	  (count-length (cdr list) (inc count)))))

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
	  
