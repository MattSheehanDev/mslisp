(define tolerance 10e-7)

(define sqrt
    (lambda (y)
      (sqrt-iter y 1)))

(define sqrt-iter
    (lambda (y guess)
      (if (close-enough guess y)
	  guess
	  (sqrt-iter y (newton guess y)))))

(define newton
    (lambda (guess y)
      (/ (+ guess (/ y guess)) 2.0)))

(define close-enough
    (lambda (guess y)
      (< (abs (- (square guess) y)) tolerance)))

(define square
    (lambda (x)
      (* x x)))

(define abs
    (lambda (x)
      (if (> x 0)
	  x
	  (- 0 x))))
