;; dynamic variable to hold current test variable name.
(define *test-name* ())

;; macro to define unit-test function
(defmacro deftest (name params body)
  `(defun ,name ,params
     (lexical-let (*test-name* (cons ',name *test-name*))
       ,body)))

;; i'm not sure if this is a bug or not,
;; but a regular [let] should bind to *test-name* properly.
;; the way lambda's are implemented the lexical scope is
;; sometimes off.
(defmacro lexical-let ((var value) body)
  `(let (p ,var)
     (begin
      (set! ,var ,value)
      ; saving the result of body in a variable might not make sense at
      ; first (and is sort of a work-around), we want to make sure to
      ; return the result of [body] and not the result of [set!] which
      ; is always null.
      (let (r ,body)
	(begin
	 (set! ,var p)
	 r)))))

;; macro to [report] all the macros
(defmacro check (&rest tests)
  `(combine-reports
    ,@((Y
       (lambda (f)
	 (lambda (x)
	   (cond
	     ((null? x) ())
	     (#t (cons `(report ,(car x) ',(car x)) (f (cdr x))))))))
       tests)))

;; print results
(defun report (test form)
  (begin
   (if (null? test)
       (print "FAIL ... ")
       (print "pass ... "))
   (print-test-name)
   (print (format "{0}" form))
   (print-line "")
   (not (null? test))))

;; print *test-name* variable
(defun print-test-name ()
    (loop *test-name* (lambda (name)
		     (print (format "{0}: " name)))))

(defmacro combine-reports (&rest list)
  `(let (result #t)
    (begin
     ,@((Y
	(lambda (f)
	  (lambda (x)
	    (cond
	      ((null? x) ())
	      (#t (cons `(,unless ,(car x) (set! result ,())) (f (cdr x))))))))
       list)
     result)))

;;;;
;;;; define unit tests below here
;;;;


; test complete std.lisp
(deftest test-std ()
  (combine-reports
   (test-types)
   (test-arithmetic)
   (test-conditionals)
   (test-listparts)))

;; test if types work
(deftest test-types ()
  (combine-reports
   (test-atom?)
   (test-number?)
   (test-list?)
   (test-null?)))

(deftest test-atom? ()
  (check
    (eq? (atom? "atom") #t)
    (eq? (atom? 10) #t)
    (eq? (atom? 'atom) #t)
    (eq? (atom? '(list)) nil)
    (eq? (atom? ()) nil)))

(deftest test-number? ()
  (check
    (eq? (number? 10) #t)
    (eq? (number? 5.5) #t)
    (eq? (number? "number") nil)
    (eq? (number? 'number) nil)
    (eq? (number? ()) nil)))

(deftest test-list? ()
  (check
    (eq? (list? ()) #t)
    (eq? (list? '(1 2 3)) #t)
    (eq? (list? #t) nil)
    (eq? (list? 7) nil)
    (eq? (list? 'list) nil)
    (eq? (list? '(list)) #t)))

(deftest test-null? ()
  (check
    (eq? (null? ()) #t)
    (eq? (null? nil) #t)
    (eq? (null? '()) #t)
    (eq? (null? (quote ())) #t)
    (eq? (null? 10) nil)
    (eq? (null? "null") nil)
    (eq? (null? #t) nil)))


; test arithmetic package
(deftest test-arithmetic ()
  (combine-reports
   (test-operators)
   (test-comparisons)))

; test arithmetic operators
(deftest test-operators ()
  (combine-reports
   (test+)
   (test-)
   (test/)
   (test*)))

(deftest test+ ()
  (check
   (= (+ 5) 5)
   (= (+ 1 2) 3)
   (= (+ 1 2 3) 6)
   (= (+ -1 -3) -4)))

(deftest test- ()
  (check
   (= (- 5) -5)
   (= (- 1 2) -1)
   (= (- 1 2 3) -4)
   (= (- -1 -3) 2)))

(deftest test/ ()
  (check
    (= (/ 4) 0.25)
    (= (/ 10 5) 2)
    (= (/ 100 5 2) 10)
    (= (/ -10 -2) 5)
    (= (/ -10 2) -5)))

(deftest test* ()
  (check
    (= (* 2) 2)
    (= (* 2 2) 4)
    (= (* 5 4 3 2 1) 120)
    (= (* -7 3) -21)
    (= (* -5 -5) 25)))

; test arithmetic comparisons
(deftest test-comparisons ()
  (combine-reports
   (test->)
   (test-<)
   (test->=)
   (test-<=)
   (test-=)))

(deftest test-> ()
  (check
    (eq? (> 5) #t)
    (eq? (> 5 4) #t)
    (eq? (> 5 4 3 2 1) #t)
    (eq? (> 5 4 3 1 2) nil)
    (eq? (> 1 2 3) nil)
    (eq? (> -1 -2 -3) #t)
    (eq? (> 1.5 1.4) #t)
    (eq? (> 1.4 1.3 1.5) nil)))

(deftest test-< ()
  (check
    (eq? (< 3) #t)
    (eq? (< 3 4) #t)
    (eq? (< 1 2 3 4 5) #t)
    (eq? (< 5 4 3 2 1) nil)
    (eq? (< -5 -4 -3) #t)
    (eq? (< 1.5 1.6) #t)
    (eq? (< 1.6 1.7 1.5) nil)))

(deftest test->= ()
  (check
    (eq? (>= 1) #t)
    (eq? (>= 3 2) #t)
    (eq? (>= -3 -5 -7) #t)
    (eq? (>= 1.5 1.5 1.4) #t)
    (eq? (>= 12 10 11) nil)
    (eq? (>= 1.6 1.6 1.5) #t)
    (eq? (>= 1.4 1.4 1.5) nil)))

(deftest test-<= ()
  (check
    (eq? (<= 1 2 3) #t)
    (eq? (<= 4) #t)
    (eq? (<= -1 0 1) #t)
    (eq? (<= -1 1 0) nil)
    (eq? (<= 3.3 3.3 3.3) #t)
    (eq? (<= 1 3 5 2 4) nil)))

(deftest test-= ()
  (check
    (eq? (= 2) #t)
    (eq? (= 3 3) #t)
    (eq? (= -1 0) nil)
    (eq? (= -1 -1 -1) #t)
    (eq? (= 8 8 8 7) nil)
    (eq? (= 1 2 3) nil)))

; test conditional statements
(deftest test-conditionals ()
  (combine-reports
   (test-not)
   (test-and)
   (test-or)))

(deftest test-not ()
  (check
    (eq? (not #t) nil)
    (eq? (not nil) #t)
    (eq? (not ()) #t)))

(deftest test-and ()
  (check
    (eq? (and #t ()) nil)
    (eq? (and #t #t) #t)
    (eq? (and 5 8) #t)
    (eq? (and nil nil) nil)
    (eq? (and "true" "false") #t)
    (eq? (and 'true 'false) #t)))

(deftest test-or ()
  (check
    (eq? (or #t nil) #t)
    (eq? (or #t #t) #t)
    (eq? (or nil nil) nil)
    (eq? (or nil #t) #t)
    (eq? (or 5 8) #t)
    (eq? (or "true" nil) #t)
    (eq? (or nil 'false) #t)))

; test variable equality comparisons
(deftest test-equality ()
  (combine-reports
   (test-equan?)
   (test-eqlist?)
   (test-equal?)))

(deftest test-equan? ()
  (check
    (eq? (equan? 5 5) #t)
    (eq? (equan? "five" "five") #t)
    (eq? (equan? 'five 'five) #t)
    (eq? (equan? 10 5) nil)
    (eq? (equan? '(1 2 3) '(1 2 3)) nil)))

(deftest test-eqlist? ()
  (check
    (eq? (eqlist? () ()) #t)
    (eq? (eqlist? () '(1 2 3)) nil)
    (eq? (eqlist? '(1 2 3) '()) nil)
    (eq? (eqlist? '(1 2 3) '(1 2 3)) #t)
    (eq? (eqlist? '((1 2 3) 4 5) '((1 2 3) 4 5)) #t)
    (eq? (eqlist? '(1 2 3 (4 5)) '(1 2 3 4 5)) nil)))

(deftest test-equal? ()
  (check
    (eq? (equal? 10 10) #t)
    (eq? (equal? 'ten 'ten) #t)
    (eq? (equal? '((1 2 3) 4 5) '((1 2 3) 4 5)) #t)
    (eq? (equal? () nil) #t)))


; test list position helpers
(deftest test-listparts ()
  (combine-reports
   (test-cdar)
   (test-caar)
   (test-cddr)
   (test-cadr)
   (test-cadar)
   (test-caddr)
   (test-caddar)
   (test-nth)
   (test-last)))

(deftest test-cdar ()
  (check
    (equal? (cdar '((1 2 3) 4 5)) '(2 3))
    (equal? (cdar '(() a b)) ())
    (equal? (cdar '((a) b c)) ())))

(deftest test-caar ()
  (check
    (equal? (caar '((1 2 3) 4 5)) 1)
    (equal? (caar '((a) b c)) 'a)
    (equal? (caar '(() "string")) ())))

(deftest test-cddr ()
  (check
    (equal? (cddr ()) ())
    (equal? (cddr '(a b c)) '(c))
    (equal? (cddr '((a b) (c d) (e f))) '((e f)))
    (equal? (cddr '((1) 2)) ())))

(deftest test-cadr ()
  (check
    (equal? (cadr ()) ())
    (equal? (cadr '(1 2 3)) 2)
    (equal? (cadr '(a (b c))) '(b c))))

(deftest test-cadar ()
  (check
    (equal? (cadar ()) ())
    (equal? (cadar '((a b c) d)) 'b)
    (equal? (cadar '((a) b c)) ())
    (equal? (cadar '((a (b)) c)) '(b))))

(deftest test-caddr ()
  (check
    (equal? (caddr ()) ())
    (equal? (caddr '(1 2 3)) 3)
    (equal? (caddr '(a (b c) (d e))) '(d e))
    (equal? (caddr '(() () (()))) '(()))))

(deftest test-caddar ()
  (check
    (equal? (caddar '((a b c) d)) 'c)
    (equal? (caddar '((a b (c d)) e)) '(c d))))

(deftest test-nth ()
  (check
    (equal? (nth () 0) ())
    (equal? (nth '(1 2 3) 2) 3)
    (equal? (nth '(a (b c) d) 1) '(b c))
    (equal? (nth '("one" "two") 0) "one")))

(deftest test-last ()
  (check
    (equal? (last ()) ())
    (equal? (last '(1 2 3)) 3)
    (equal? (last '((a b) (c d))) '(c d))))
