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

;;;; define unit tests below here

(deftest test-arithmetic ()
  (combine-reports
   (test+)
   (test-)
   (test/)
   (test*)))

(deftest test+ ()
  (check
   (= (+ 5) 6)
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
