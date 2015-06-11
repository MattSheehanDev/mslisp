;;; This is the standard mslisp library,
;;; currently under the filename std.lisp
;;;
;;; NOTE: I might change the name to sml.lisp (standard macro library),
;;; once macros are implemented.

(define not
    (lambda (x)
      (if (null? x) #t nil)))

(define append
    (lambda (x y)
      (if (null? x)
	  y
	  (cons (car x) (append (cdr x) y)))))

;; there is a bug in the parser where the empty list
;; created below is parsed and created once
;; and each iterative call to pair appends to the same list.
(define pair
    (lambda (x y)
      (cons x (cons y (quote ())))))

;; &rest parameters are not implemented yet,
;; but if they were [list] would be preferred
;; over [pair].
(define list
    (lambda (&rest x)
      (if (null? x)
	  (quote ())
	  (cons (car x) (list (cdr x))))))

;; the following to procedures could be combined
;; with a [let] macro.
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
	  
