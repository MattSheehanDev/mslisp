(define assert-equal
    (lambda (x y)
      (equals? x y)))

(define assert-not-equal
    (lambda (x y)
      (not (assert-equal x y))))

(assert-equal (eval 'x '((x test-value)))
	      (quote test-value))

(assert-not-equal (eval 'y '((y (1 2 3))))
	      (quote (1 2 3)))

(assert-not-equal (eval 'z '((z ((1) 2 3))))
	      '((1) 2 3))

(assert-equal (eval 7 nil)
	      7)

(assert-equal (eval '(atom? (quote (1 2))) nil)
	      nil)

(assert-equal (eval '(equals? 1 1) nil)
	      #t)

(assert-equal (eval '(equals? 1 2) nil)
	      nil)

(assert-equal (eval '(car x) '((x (3 2))))
	      3)

(assert-not-equal (eval '(cdr (quote (1 2 3))) nil)
		  (pair 2 3))

(assert-equal (eval '((lambda (x) (car (cdr x))) '(1 2 3 4)) nil)
	      2)

(assert-equal (eval '(caddr '(1 2 3)) '((caddr (lambda (x) (car (cdr (cdr x)))))))
	      3)
