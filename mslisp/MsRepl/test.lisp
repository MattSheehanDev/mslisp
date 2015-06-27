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
      ,body
      (set! ,var p))))

;; macro to [report] all the macros
(defmacro check (&rest tests)
  `(begin
    ,@((Y
       (lambda (f)
	 (lambda (x)
	   (if (not (null? (car x)))
	       (cons `(report ,(car x) ',(car x)) (f (cdr x)))
	       nil))))
      tests)))

;; print results
(defun report (test form)
  (if (null? test)
      (begin
       (print (format "FAIL {0}: " test))
       (print-test-name ()))
      (begin
       (print (format "pass {0}: " test))
       (print-test-name ())
       (print (length *test-name*))
       (print (format "{0}" form))
       (print-line ""))))

;; print *test-name* variable
(defun print-test-name ()
    (loop *test-name* (lambda (name)
		     (print (format "{0} " name)))))


;;;; define unit tests below here

(deftest test+ ()
  (check
   (= (+ 5) 5)
   (= (+ 1 2) 3)
   (= (+ 1 2 3) 6)
   (= (+ -1 -3) -4)))
