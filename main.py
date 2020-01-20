import json
import numpy as np
import matplotlib.pyplot as plt
import glob
import os

if __name__ == "__main__":
    files_count = len(glob.glob1(os.curdir, "*.json"))
    generations = []
    for i in range(1, files_count + 1):
        with open('gen' + str(i) + '.json') as json_file:
            generations.append(json.load(json_file))

    # Score
    bests = []
    worsts = []
    avgs = []
    variance = []
    # Time
    best_time = []
    worst_time = []
    avg_time = []
    var_time = []
    # Collision
    best_collision = []
    worst_collision = []
    avg_collision = []
    var_collision = []

    gen_no = np.arange(1, files_count + 1)
    for gen in generations:
        gen_times = []
        gen_collision = []
        for chromosome in gen['population']:
            gen_times.append(chromosome['timeScore'])
            gen_collision.append(chromosome['collisionScore'])
        gen_times = np.array(gen_times)
        gen_collision = 1 / (1 + np.array(gen_collision))
        gen_scores = gen_times * gen_collision
        # Score
        bests.append(np.max(gen_scores))
        worsts.append(np.min(gen_scores))
        avgs.append(np.average(gen_scores))
        variance.append(np.var(gen_scores))
        # Time
        best_time.append(np.max(gen_times))
        worst_time.append(np.min(gen_times))
        avg_time.append(np.average(gen_times))
        var_time.append(np.var(gen_times))
        # Collision
        best_collision.append(np.max(gen_collision))
        worst_collision.append(np.min(gen_collision))
        avg_collision.append(np.average(gen_collision))
        var_collision.append(np.var(gen_collision))
    # Score
    plt.plot(gen_no, bests, label="Best")
    plt.plot(gen_no, worsts, label="Worst")
    plt.plot(gen_no, avgs, label="Average")
    plt.xlabel('Generation')
    plt.ylabel('Score')
    plt.legend()
    plt.show()
    # Time Score
    plt.plot(gen_no, best_time, label="Best")
    plt.plot(gen_no, worst_time, label="Worst")
    plt.plot(gen_no, avg_time, label="Average")
    plt.xlabel('Generation')
    plt.ylabel('Time score')
    plt.legend()
    plt.show()
    # Collision Score
    plt.plot(gen_no, best_collision, label="Best")
    plt.plot(gen_no, worst_collision, label="Worst")
    plt.plot(gen_no, avg_collision, label="Average")
    plt.xlabel('Generation')
    plt.ylabel('Collision score')
    plt.legend()
    plt.show()
    # Variance
    plt.plot(gen_no, variance, label="Score")
    plt.plot(gen_no, var_time, label="Time score")
    plt.plot(gen_no, var_collision, label="Collision score")
    plt.xlabel('Generation')
    plt.ylabel('Variance')
    plt.legend()
    plt.show()
